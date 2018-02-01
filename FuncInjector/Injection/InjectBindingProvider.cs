using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;

namespace FuncInjector
{
    public class InjectBindingProvider : IBindingProvider
    {
        private readonly InjectorConfigTrigger _configuration;

        public InjectBindingProvider(InjectorConfigTrigger configuration)
        {
            _configuration = configuration;
        }
            

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute =
                parameter.GetCustomAttribute<InjectAttribute>(false);
            
            IBinding binding = new InjectBinding(context.Parameter.ParameterType, _configuration, attribute.InjectConfigFunctionName);
            return Task.FromResult(binding);
        }
    }
}
