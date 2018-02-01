using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FuncInjector
{

    public class InjectBinding : IBinding
    {
        private readonly Type _type;
        private readonly InjectorConfigTrigger _configuration;
        private readonly string _configFunctionName;

        public InjectBinding(Type type, InjectorConfigTrigger configuration, string configFunctionName)
        {
            _type = type;
            _configuration = configuration;
            _configFunctionName = configFunctionName;
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) =>
            Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            await Task.Yield();
            var provider = _configuration.GetServiceProvider(_configFunctionName);
            var scope = _configuration.Scopes.GetOrAdd(context.FunctionInstanceId, (_) => provider.CreateScope());
            var value = scope.ServiceProvider.GetRequiredService(_type);
            return await BindAsync(value, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();

        private class InjectValueProvider : IValueProvider
        {
            private readonly object _value;

            public InjectValueProvider(object value) => _value = value;

            public Type Type => _value.GetType();

            public Task<object> GetValueAsync() => Task.FromResult(_value);

            public string ToInvokeString() => _value.ToString();
        }
    }
}
