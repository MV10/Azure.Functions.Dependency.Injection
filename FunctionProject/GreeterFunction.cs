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
        [FunctionName("TickTock")]
        public static void Run0(
            [TimerTrigger("*/15 * * * * *")] TimerInfo myTimer, // every 15 sec
            [Inject("TickTockConfig")]ITickTock ticker,
            TraceWriter log)
        {
            log.Info($"{ticker.TickOrTock()} ... C# Timer trigger function executed at: {DateTime.Now}");
        }

        [FunctionName("GreeterSingleton1")]
        public static async Task<HttpResponseMessage> Run1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
            [Inject("SingletonConfig")]IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("GreeterSingleton2")]
        public static async Task<HttpResponseMessage> Run2(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
           [Inject("SingletonConfig")]IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("GreeterScope")]
        public static async Task<HttpResponseMessage> Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
            [Inject("ScopeConfig")]IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("GreeterTransient")]
        public static async Task<HttpResponseMessage> Run4(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
           [Inject("ScopeTransient")]IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }

        [FunctionName("SingletonConfig")]
        public static void Config1([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterSingleton();
        }

        [FunctionName("ScopeConfig")]
        public static void Config2([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterScoped();
        }

        [FunctionName("ScopeTransient")]
        public static void Config3([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddGreeterTransient();
        }

        [FunctionName("TickTockConfig")]
        public static void Config4([InjectorConfigTrigger] IServiceCollection services)
        {
            services.AddTickTockTransient();
        }
    }
}
