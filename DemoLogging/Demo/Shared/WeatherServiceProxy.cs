using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared
{
  public class WeatherServiceProxy : IWeatherService
  {
    private readonly HttpClient httpClient;

    public WeatherServiceProxy(HttpClient httpClient)
    {
      this.httpClient = httpClient;
    }

    public async ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate)
    {
      return await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("weatherforecast");
    }
  }
}
