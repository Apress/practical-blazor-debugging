using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DebuggingFeatures.Shared;
using DebuggingFeatures.Blazor.WebAssembly.Services;
using DebuggingFeatures.Components;

namespace DebuggingFeatures.Blazor.WebAssembly
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("app");

      builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
      builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
      builder.Services.AddLocalStorage();
      await builder.Build().RunAsync();
    }
  }
}
