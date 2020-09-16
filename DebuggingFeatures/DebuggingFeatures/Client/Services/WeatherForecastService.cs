using DebuggingFeatures.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DebuggingFeatures.Blazor.WebAssembly.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient httpClient;

        public WeatherForecastService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            return httpClient.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
        }
    }
}
