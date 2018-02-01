using Microsoft.Azure.WebJobs.Description;
using System;

namespace FuncInjector
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public string InjectConfigFunctionName { get; private set; }

        public InjectAttribute(string configFunctionName)
        {
            InjectConfigFunctionName = configFunctionName;
        }
    }
}
