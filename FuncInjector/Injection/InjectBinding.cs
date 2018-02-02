using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FuncInjector
{
    /// <summary>
    /// Tracks the Type declared as a dependency and the RegisterServices trigger which
    /// prepares the dependency graph for injection. The trigger is stored as the value-provider,
    /// so resolving the dependency causes trigger execution.
    /// </summary>
    public class InjectBinding : IBinding
    {
        private readonly Type type;
        private readonly RegisterServicesTrigger triggerAttrib;
        private readonly string triggerFunction;

        public InjectBinding(Type type, RegisterServicesTrigger regSvcTrigger, string regSvcFunctionName)
        {
            this.type = type;
            triggerAttrib = regSvcTrigger;
            triggerFunction = regSvcFunctionName;
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) 
            => Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            var provider = triggerAttrib.GetServiceProvider(triggerFunction);
            var scope = triggerAttrib.Scopes.GetOrAdd(context.FunctionInstanceId, (_) => provider.CreateScope());
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
