using DebuggingFeatures.Blazor.Server.Data;
using DebuggingFeatures.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace DebuggingFeatures.Weather.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeatherServicesForServer(this IServiceCollection services)
        {
            return services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        }
    }
}
