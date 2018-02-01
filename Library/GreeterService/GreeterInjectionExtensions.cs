using Microsoft.Extensions.DependencyInjection;

namespace Library
{
    public static class GreeterInjectionExtensions
    {
        public static IServiceCollection AddGreeterSingleton(this IServiceCollection services)
        {
            services.AddSingleton<ICounter, Counter>();
            services.AddSingleton<IGreeter, CountUpGreeter>();
            return services;
        }

        public static IServiceCollection AddGreeterScoped(this IServiceCollection services)
        {
            services.AddScoped<ICounter, Counter>();
            services.AddScoped<IGreeter, CountUpGreeter>();
            return services;
        }

        public static IServiceCollection AddGreeterTransient(this IServiceCollection services)
        {
            services.AddTransient<ICounter, Counter>();
            services.AddTransient<IGreeter, CountUpGreeter>();
            return services;
        }
    }
}
