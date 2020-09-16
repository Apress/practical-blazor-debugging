using Demo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private readonly IWeatherService weatherService;
    private readonly ILogger<WeatherForecastController> logger;

    public WeatherForecastController(IWeatherService weatherService, ILogger<WeatherForecastController> logger)
    {
      this.weatherService = weatherService;
      this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
      return await weatherService.GetAsync(DateTime.Now);
    }
  }
}
