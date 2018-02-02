using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using System;

using FuncInjector;
using Library;

namespace FunctionProject
{
    public static class GreeterFunction
    {
        [FunctionName("GreeterSingleton1")]
        public static async Task<HttpResponseMessage> Run1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [Inject("RegisterSingletons")] IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("GreeterSingleton2")]
        public static async Task<HttpResponseMessage> Run2(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
           [Inject("RegisterSingletons")] IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("GreeterScoped")]
        public static async Task<HttpResponseMessage> Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [Inject("RegisterScoped")] IGreeter greeter,
            [Inject("RegisterScoped")] IGreeterConsumer scoped)
        {
            return req.CreateResponse($"greetersvc {greeter.Greet()} --- scopedsvc {scoped.Greeting()}");
        }

        [FunctionName("GreeterNonScoped")]
        public static async Task<HttpResponseMessage> Run4(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [Inject("RegisterNonScoped")] IGreeter greeter,
            [Inject("RegisterNonScoped")] IGreeterConsumer scoped)
        {
            return req.CreateResponse($"greetersvc {greeter.Greet()} --- scopedsvc {scoped.Greeting()}");
        }

        [FunctionName("GreeterTransient")]
        public static async Task<HttpResponseMessage> Run5(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
           [Inject("RegisterTransient")] IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("RegisterSingletons")]
        public static void Config1([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterSingleton();
        }

        [FunctionName("RegisterScoped")]
        public static void Config2([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterScoped();
            services.AddGreeterConsumer();
        }

        [FunctionName("RegisterNonScoped")]
        public static void Config3([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterTransient();
            services.AddGreeterConsumer();
        }

        [FunctionName("RegisterTransient")]
        public static void Config4([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterTransient();
        }
    }
}
