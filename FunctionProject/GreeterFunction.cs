using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using FuncInjector;
using Library;

namespace FunctionProject
{
    public static class GreeterFunction
    {
        [FunctionName("GreeterSingleton1")]
        public static async Task<IActionResult> Run1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [Inject("RegisterSingletons")] IGreeter greeter)
        {
            return new OkObjectResult(greeter.Greet());
        }

        [FunctionName("GreeterSingleton2")]
        public static async Task<IActionResult> Run2(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
           [Inject("RegisterSingletons")] IGreeter greeter)
        {
            return new OkObjectResult(greeter.Greet());
        }

        [FunctionName("GreeterScoped")]
        public static async Task<IActionResult> Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [Inject("RegisterScoped")] IGreeter greeter,
            [Inject("RegisterScoped")] IGreeterConsumer scoped)
        {
            return new OkObjectResult($"greetersvc {greeter.Greet()} --- scopedsvc {scoped.Greeting()}");
        }

        [FunctionName("GreeterNonScoped")]
        public static async Task<IActionResult> Run4(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [Inject("RegisterNonScoped")] IGreeter greeter,
            [Inject("RegisterNonScoped")] IGreeterConsumer scoped)
        {
            return new OkObjectResult($"greetersvc {greeter.Greet()} --- scopedsvc {scoped.Greeting()}");
        }

        [FunctionName("GreeterTransient")]
        public static async Task<IActionResult> Run5(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
           [Inject("RegisterTransient")] IGreeter greeter)
        {
            return new OkObjectResult(greeter.Greet());
        }

        [FunctionName("GreeterDefaultReg")]
        public static async Task<IActionResult> Run6(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
           [Inject] IGreeter greeter)
        {
            return new OkObjectResult(greeter.Greet());
        }

        [FunctionName("RegisterSingletons")]
        public static void Config1([RegisterServicesTrigger] IServiceCollection services)
        {
            services.AddGreeterSingleton();
        }

        [FunctionName("RegisterScoped")]
        public static void Config2([RegisterServicesTrigger] IServiceCollection services)
        {
            services.AddGreeterScoped();
            services.AddGreeterConsumer();
        }

        [FunctionName("RegisterNonScoped")]
        public static void Config3([RegisterServicesTrigger] IServiceCollection services)
        {
            services.AddGreeterTransient();
            services.AddGreeterConsumer();
        }

        [FunctionName("RegisterTransient")]
        public static void Config4([RegisterServicesTrigger] IServiceCollection services)
        {
            services.AddGreeterTransient();
        }

        [FunctionName("RegisterServices")] // default registration trigger name for [Inject]
        public static void Config5([RegisterServicesTrigger] IServiceCollection services)
        {
            services.AddGreeterTransient();
        }
    }
}
