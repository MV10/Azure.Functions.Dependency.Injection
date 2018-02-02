using Microsoft.Azure.WebJobs.Description;
using System;

namespace FuncInjector
{
    /// <summary>
    /// Declaration of the RegisterServicesTrigger attribute.
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class RegisterServicesTriggerAttribute : Attribute { }
}