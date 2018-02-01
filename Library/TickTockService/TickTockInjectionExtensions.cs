using Microsoft.Extensions.DependencyInjection;

namespace Library
{
    public static class TickTockInjectionExtensions
    {
        public static IServiceCollection AddTickTockSingleton(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<ITickTock, TickTock>();
            return services;
        }

        public static IServiceCollection AddTickTockScoped(this IServiceCollection services)
        {
            services.AddScoped<IClock, Clock>();
            services.AddScoped<ITickTock, TickTock>();
            return services;
        }

        public static IServiceCollection AddTickTockTransient(this IServiceCollection services)
        {
            services.AddTransient<IClock, Clock>();
            services.AddTransient<ITickTock, TickTock>();
            return services;
        }
    }
}
