using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
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
        private readonly bool isArray =false;
        private readonly bool hasDefaultValue = false;
        private readonly object defaultValue;

        private readonly Type elementType;
        private readonly RegisterServicesTrigger triggerAttribute;
        private readonly string triggerFunction;

        /// <summary>
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <param name="regSvcTrigger"></param>
        /// <param name="regSvcFunctionName"></param>
        public InjectBinding(ParameterInfo parameterInfo, RegisterServicesTrigger regSvcTrigger, string regSvcFunctionName)
        {
            this.hasDefaultValue = parameterInfo.HasDefaultValue;
            this.defaultValue = parameterInfo.DefaultValue;
            
            this.isArray = parameterInfo.ParameterType.IsArray;
            this.elementType = parameterInfo.ParameterType.IsArray ? 
                parameterInfo.ParameterType.GetElementType() :
                parameterInfo.ParameterType.GetType();

            triggerAttribute = regSvcTrigger;
            triggerFunction = regSvcFunctionName;
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) 
            => Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            var provider = triggerAttribute.GetServiceProvider(triggerFunction);
            var scope = triggerAttribute.Scopes.GetOrAdd(context.FunctionInstanceId, (_) => provider.CreateScope());


            var services = hasDefaultValue ?
               (
                   isArray ?
                       BindAsync(
                           scope.ServiceProvider.GetServices(elementType),
                           context.ValueContext
                       )
                       :
                       BindAsync(
                           scope.ServiceProvider.GetService(elementType),
                           context.ValueContext
                       )
               ) : (
                   isArray ?
                       BindAsync(
                           scope.ServiceProvider.GetServices(elementType),
                           context.ValueContext
                       )
                       :
                       BindAsync(
                           scope.ServiceProvider.GetRequiredService(elementType),
                           context.ValueContext
                       )
               );
            return await services;
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
