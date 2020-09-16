using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DebuggingFeatures.Shared
{
  [System.Diagnostics.DebuggerDisplay("{Date.DayOfWeek} {Date,nq} {TemperatureC}")]
  public class WeatherForecast
  {
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
  }
}
