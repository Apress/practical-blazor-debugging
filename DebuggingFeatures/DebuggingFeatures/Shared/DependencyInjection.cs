using Microsoft.Extensions.DependencyInjection;

namespace DebuggingFeatures.Shared
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddWeatherProcessor(this IServiceCollection services) 
      => services.AddSingleton<WeatherProcessor>();
  }
}
