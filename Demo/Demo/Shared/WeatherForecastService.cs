using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared
{
  public class WeatherForecastService : IWeatherService
  {
    private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    public async ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate)
    {
      var rng = new Random(101);
      return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = startDate.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      }).ToArray());
    }
  }
}
