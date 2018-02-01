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
        private readonly Dictionary<string, Type> bindingContract 
            = new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"data", typeof(IServiceCollection)}
            };

        private readonly InjectorConfigTrigger configuration;
        private readonly ParameterInfo parameter;
        public InjectorConfigTriggerBinding(ParameterInfo parameter, InjectorConfigTrigger configuration)
        {
            this.parameter = parameter;
            this.configuration = configuration;
        }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            if(value is IServiceCollection)
            {
                IValueBinder binder = new InjectorConfigTriggerValueBinder(value as IServiceCollection, parameter.ParameterType);
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
            configuration.AddConfigExecutor(context.Descriptor.ShortName, context.Executor);
            return Task.FromResult((IListener)new InjectorConfigTriggerListener());
        }

        public ParameterDescriptor ToParameterDescriptor() => new InjectorConfigTriggerParameterDescriptor();

        public Type TriggerValueType => typeof(IServiceCollection);

        public IReadOnlyDictionary<string, Type> BindingDataContract => bindingContract;

        private class InjectorConfigTriggerValueBinder : ValueBinder, IDisposable
        {
            private readonly IServiceCollection services;
            public InjectorConfigTriggerValueBinder(IServiceCollection serviceCollection, Type type, BindStepOrder bindStepOrder = BindStepOrder.Default) : base(type, bindStepOrder)
                => services = serviceCollection;

            public void Dispose() { }

            public override Task<object> GetValueAsync() => Task.FromResult((object)services);

            public override string ToInvokeString() => string.Empty;
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
