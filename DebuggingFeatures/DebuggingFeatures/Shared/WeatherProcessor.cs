using System;

namespace DebuggingFeatures.Shared
{
  public class WeatherProcessor
  {
    private const decimal KelvinToCelcius = 273.15M;

    public void Process(WeatherForecast forecast)
    {
      int temperatureC = Math.Max(forecast.TemperatureC, 0);
      decimal factor = KelvinToCelcius / temperatureC;
      // ... more logic
    }
  }
}
