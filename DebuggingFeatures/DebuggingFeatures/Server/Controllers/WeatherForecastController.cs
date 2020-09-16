using DebuggingFeatures.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DebuggingFeatures.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private readonly IWeatherForecastService forecastService;
    private readonly ILogger<WeatherForecastController> logger;

    public WeatherForecastController(IWeatherForecastService forecastService, ILogger<WeatherForecastController> logger)
    {
      this.forecastService = forecastService;
      this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get() 
      => await this.forecastService.GetForecastAsync(DateTime.Now);
  }
}
