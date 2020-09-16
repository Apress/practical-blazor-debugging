using System;
using System.Threading.Tasks;

namespace DebuggingFeatures.Shared
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
    }
}
