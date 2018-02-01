using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FuncInjector
{

    public class InjectBinding : IBinding
    {
        private readonly Type type;
        private readonly InjectorConfigTrigger trigger;
        private readonly string configfunction;

        public InjectBinding(Type type, InjectorConfigTrigger configuration, string configFunctionName)
        {
            this.type = type;
            trigger = configuration;
            configfunction = configFunctionName;
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) 
            => Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            var provider = trigger.GetServiceProvider(configfunction);
            var scope = trigger.Scopes.GetOrAdd(context.FunctionInstanceId, (_) => provider.CreateScope());
            var value = scope.ServiceProvider.GetRequiredService(type);
            return await BindAsync(value, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();

        private class InjectValueProvider : IValueProvider
        {
            private readonly object value;

            public InjectValueProvider(object value) => this.value = value;

            public Type Type => value.GetType();

            public Task<object> GetValueAsync() => Task.FromResult(value);

            public string ToInvokeString() => value.ToString();
        }
    }
}
