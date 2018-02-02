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
    /// <summary>
    /// Initialization and Lazy storage of all RegisterServices triggers.
    /// </summary>
    public class RegisterServicesTrigger : IExtensionConfigProvider
    {
        public readonly ConcurrentDictionary<Guid, IServiceScope> Scopes = new ConcurrentDictionary<Guid, IServiceScope>();

        private readonly ConcurrentDictionary<string, Lazy<IServiceProvider>> configFnExecs = new ConcurrentDictionary<string, Lazy<IServiceProvider>>();

        public void Initialize(ExtensionConfigContext context)
        {
            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider(this));
            var registry = context.Config.GetService<IExtensionRegistry>();
            var filter = new ReleaseScopedServicesFilter(this);
            registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
            registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
            context.Config.RegisterBindingExtensions(new RegisterServicesTriggerBindingProvider(this));
        }

        public void AddConfigExecutor(string functionName, ITriggeredFunctionExecutor executor)
        {
            var lazy = new Lazy<IServiceProvider>(() =>
            {
                var services = new ServiceCollection();
                executor.TryExecuteAsync(new TriggeredFunctionData() {TriggerValue = services}, CancellationToken.None).GetAwaiter().GetResult();
                return services.BuildServiceProvider();
            });
            configFnExecs.TryAdd(functionName, lazy);
        }

        public IServiceProvider GetServiceProvider(string functionName)
        {
            if (configFnExecs.TryGetValue(functionName, out var result))
            {
                return result.Value;
            }
            throw new Exception($"RegisterServicesTrigger function {functionName} not found.");
        }
    }
}
