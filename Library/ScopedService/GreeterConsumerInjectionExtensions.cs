using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public static class GreeterConsumerInjectionExtensions
    {
        public static IServiceCollection AddGreeterConsumer(this IServiceCollection services)
        {
            services.AddTransient<IGreeterConsumer, GreeterConsumer>();
            return services;
        }
    }
}
