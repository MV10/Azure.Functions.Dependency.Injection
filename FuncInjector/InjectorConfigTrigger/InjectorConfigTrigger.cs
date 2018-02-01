using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.DependencyInjection;

namespace FuncInjector
{
    public class InjectorConfigTrigger : IExtensionConfigProvider
    {
        public readonly ConcurrentDictionary<Guid, IServiceScope> Scopes = new ConcurrentDictionary<Guid, IServiceScope>();

        private readonly ConcurrentDictionary<string, Lazy<IServiceProvider>> configFnExecs = new ConcurrentDictionary<string, Lazy<IServiceProvider>>();

        public void Initialize(ExtensionConfigContext context)
        {
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider(true);
            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider(this));
            var registry = context.Config.GetService<IExtensionRegistry>();
            var filter = new ScopeCleanupFilter(this);
            registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
            registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
            context.Config.RegisterBindingExtensions(new InjectorConfigTriggerBindingProvider(this));
        }

        public void AddConfigExecutor(string functionName, ITriggeredFunctionExecutor executor)
        {
            var lazy = new Lazy<IServiceProvider>(() =>
            {
                var serviceCollection = CreateServiceCollection();
                executor.TryExecuteAsync(new TriggeredFunctionData() {TriggerValue = serviceCollection}, CancellationToken.None).GetAwaiter().GetResult();
                return serviceCollection.BuildServiceProvider();
            });
            configFnExecs.TryAdd(functionName, lazy);
        }

        public IServiceProvider GetServiceProvider(string functionName)
        {
            if (configFnExecs.TryGetValue(functionName, out var result))
            {
                return result.Value;
            }
            throw new Exception("Not Found ConfigFunction");
        }
        
        protected virtual IServiceCollection CreateServiceCollection() => new ServiceCollection();
    }
}
