using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared
{
    public static class DependencyInjection
    {
    public static IServiceCollection AddWeatherServiceForServer(this IServiceCollection services)
    {
      return services.AddTransient<IWeatherService, WeatherForecastService>();
    }
    public static IServiceCollection AddWeatherServiceForWebAssembly(this IServiceCollection services)
    {
      return services.AddTransient<IWeatherService, WeatherServiceProxy>();
    }
  }
}
