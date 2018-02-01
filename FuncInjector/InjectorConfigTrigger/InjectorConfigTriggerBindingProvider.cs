using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.DependencyInjection;

namespace FuncInjector
{
    public class InjectorConfigTriggerBindingProvider : ITriggerBindingProvider
    {
        private readonly InjectorConfigTrigger configuration;

        public InjectorConfigTriggerBindingProvider(InjectorConfigTrigger configuration) => this.configuration = configuration;

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if(context == null) throw new ArgumentNullException("context");
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<InjectorConfigTriggerAttribute>(false);
            if(attribute == null) return Task.FromResult<ITriggerBinding>(null);
            if(!IsSupportBindingType(parameter.ParameterType)) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Can't bind InjectorConfigTriggerAttribute to type '{0}'.", parameter.ParameterType));
            return Task.FromResult<ITriggerBinding>(new InjectorConfigTriggerBinding(context.Parameter, configuration));
        }

        public bool IsSupportBindingType(Type t) => t == typeof(IServiceCollection);
    }
}
