using Microsoft.Azure.WebJobs.Host.Bindings;
using System.Reflection;
using System.Threading.Tasks;

namespace FuncInjector
{
    public class InjectBindingProvider : IBindingProvider
    {
        private readonly InjectorConfigTrigger config;

        public InjectBindingProvider(InjectorConfigTrigger configuration) => config = configuration;

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<InjectAttribute>(false);
            IBinding binding = new InjectBinding(context.Parameter.ParameterType, config, attribute.InjectorConfigFunctionName);
            return Task.FromResult(binding);
        }
    }
}
