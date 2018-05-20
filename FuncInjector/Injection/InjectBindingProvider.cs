using Microsoft.Azure.WebJobs.Host.Bindings;
using System.Reflection;
using System.Threading.Tasks;

namespace FuncInjector
{
    /// <summary>
    /// The factory for InjectBinding.
    /// </summary>
    public class InjectBindingProvider : IBindingProvider
    {
        private readonly RegisterServicesTrigger config;

        public InjectBindingProvider(RegisterServicesTrigger configuration) => config = configuration;

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<InjectAttribute>(false);
            IBinding binding = new InjectBinding(context.Parameter, config, attribute.RegisterServicesFunctionName);
            return Task.FromResult(binding);
        }
    }
}
