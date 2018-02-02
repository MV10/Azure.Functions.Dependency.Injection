using Microsoft.Azure.WebJobs.Description;
using System;

namespace FuncInjector
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class RegisterServicesTriggerAttribute : Attribute { }
}