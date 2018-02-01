using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.DependencyInjection;
namespace FuncInjector
{
    public class InjectorConfigTriggerBinding : ITriggerBinding
    {
        private readonly InjectorConfigTrigger _configuration;
        private readonly Dictionary<string, Type> _bindingContract;
        private readonly ParameterInfo _parameter;

        public InjectorConfigTriggerBinding(ParameterInfo parameter, InjectorConfigTrigger configuration)
        {
            _parameter = parameter;
            _configuration = configuration;
            _bindingContract = new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"data", typeof(IServiceCollection)}
            };
        }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            if(value is IServiceCollection)
            {
                IValueBinder binder =
                    new InjectorConfigTriggerValueBinder(value as IServiceCollection, _parameter.ParameterType);

                return Task.FromResult((ITriggerData)new TriggerData(binder,
                    new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
                    {
                        {"data", value}
                    }));
            }
            throw new NotSupportedException();
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            _configuration.AddConfigExecutor(context.Descriptor.ShortName, context.Executor);
            return Task.FromResult((IListener)new InjectorConfigTriggerListener());
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new InjectorConfigTriggerParameterDescriptor();
        }

        public Type TriggerValueType => typeof(IServiceCollection);

        public IReadOnlyDictionary<string, Type> BindingDataContract => _bindingContract;

        private class InjectorConfigTriggerValueBinder : ValueBinder, IDisposable
        {
            private readonly IServiceCollection _serviceCollection;

            public InjectorConfigTriggerValueBinder(IServiceCollection serviceCollection, Type type, BindStepOrder bindStepOrder = BindStepOrder.Default) : base(type, bindStepOrder)
            {
                _serviceCollection = serviceCollection;
            }

            public void Dispose()
            {

            }

            public override Task<object> GetValueAsync()
            {
                return Task.FromResult((object)_serviceCollection);
            }

            public override string ToInvokeString() => string.Empty;
            //{
            //    // TODO:Executor
            //    return "nono";
            //}


        }

        private class InjectorConfigTriggerParameterDescriptor : TriggerParameterDescriptor
        {
            public override string GetTriggerReason(IDictionary<string, string> arguments)
            {
                // TODO: Customize your Dashboard display string
                return string.Format("InjectorConfigTrigger trigger fired at {0}", DateTime.Now.ToString("o"));
            }
        }
    }
}
