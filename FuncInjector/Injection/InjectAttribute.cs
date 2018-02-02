using Microsoft.Azure.WebJobs.Description;
using System;

namespace FuncInjector
{
    /// <summary>
    /// Declares a dependency on an interface to be resolved at runtime. The name of a
    /// RegisterServicesTrigger is optional. If omitted, a service registration trigger
    /// named "RegisterServices" must exist.
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public string RegisterServicesFunctionName { get; private set; }

        public InjectAttribute(string configFunctionName = "RegisterServices")
        {
            RegisterServicesFunctionName = configFunctionName;
        }
    }
}
