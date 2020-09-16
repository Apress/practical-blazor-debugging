using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared
{
  public interface IWeatherService
  {
    ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate);
  }
}
