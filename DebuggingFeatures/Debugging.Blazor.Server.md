# Practical Blazor Debugging

## Introduction

Have you been debugging Blazor WebAssembly recently and found the Debugger not up to the task? Or did you have trouble
reproducing that bug from production?

Then watch this video where I will show you how to setup your project so you can use the full debugging capabilities
of Visual Studio. You will learn how to run your components both in Blazor Webassembly and Blazor Server.

Why? Because with Blazor Server you can use all the features that Visual Studio debugging has to offer, for example
conditional breakpoints, ... I will also show you a whole bunch of debugging tips and tricks so you can become
a master at debugging applications!

Topics we will cover are:

* Debugging with Blazor Server
* Debugging with Blazor WebAssembly
* Setup your project to use both Blazor WebAssembly and Blazor Server
* Advanced Breakpoints and Tracepoints
* Using the Watch and Immediate window to inspect and modify variables
* Build and debug services that support Blazor WebAssembly and Blazor Server
* Customize the debugger's behavior with attributes
* Find bugs in large loops with Hitcount breakpoints
* Use logging to reproduce those hard to find bugs






## Debugging Blazor Server

### The Blazor Server Runtime Model

In this segment of the video I want to look at debugging Blazor Server.

Let us start by reviewing the Blazor Runtime Model. 

With Blazor Server, the .NET runtime is running on the server, and uses a SignalR connection
to connect to JavaScript running in the browser.

This allows for a very small download, but at the cost of Blazor needing to be connected
to the server all the time.

For debugging this means that we can use all the debugging features of Visual Studio.

Let's have a look at debugging a Blazor Server application with Visual Studio:

### Demo

Make *DebuggingFeatures.Blazor.Server* the startup project.

I have here a project that contains components you know from the starter template for Blazor projects.

Let's run this project. 

Wait for the browser to open, now open the Browser's debugger.
Open the network tab and do a hard refresh.

As you can see, the download size for your Blazor Server application is very small.
This downloads typical files like html, css and some JavaScript.
You can also look at the SignalR connection. 
As you can see, these messages are tiny!

Now, let's put a breakpoint in the *Counter* component. Let's click the Increment button.
Visual Studio stops on the breakpoint as expected. With Visual Studio you can put a breakpoint
on any line of code in a component. Since Blazor is running in the server, this is the same
as debugging regular C# code.

We can inspect fields and properties using the Watch window, or simply by hovering the mouse
over it. You can pin these so keep track of their value.

You can drag fields and properties to the Watch window, and change their value.
This allows you to easily test your code with edge-case values which might be very hard 
to get in a normal application.

Let's look at some data that was downloaded from the server. Open the *FetchData* tab.
We can easily look at the elements of the array, again by hovering over them.
Let's look at the first forecast. Which day of the week is this?
You can type this expression `forecasts[0].Date.DayOfWeek` in the watch window.

Finally lets look at a buggy loop. When you click the *loop* button the application crashes.
Visual Studio will stop on the thrown exception. This makes it very easy to figure out which
circumstances lead to the bug.

We will look at other debugging features in the tips and tricks segment.

### Conclusion

With Blazor Server you can use all the debugging features of Visual Studio, 
because this is just another .NET application running in .NET Core.

---

## Debugging Blazor WebAssembly

In this segment we will discuss debugging Blazor WebAssembly with Visual Studio.

First, let us review the Blazor WebAssembly Runtime Model.

With Blazor WebAssembly, the dotnet runtime get loaded into the browser, which then starts to 
download your application assemblies and its dependencies. The initial download can be several
Megabytes, but next time the browser can use the cache to avoid downloading everything again.
The big advantage is that the Blazor application can now proceed without needing a server.

Because your appliction is running in the browser, the debugger needs support from the browser
to enable breakpoints, watching fields and properties, etc... Currently this support is limited
to Chromium bases browsers such as Chrome and Edge.

Open the browser's debugger first and let's look at the network tab. Do a hard reload.
As you can see the download is substantially bigger because all your applications DLLs need
to be downloaded. However, because the browser is caching a lot of files, the actual download
is a lot smaller than the application. If you refresh the browser again you can see this.

Let's use Visual Studio to debug this application running in the browser.

You need to enable debugging in the `launchSettings.json` file of your 
startup project. Here you need to add the `inspectUri`. Visual Studio will normally configure
this for you, but you will need to do this manually for older projects.

Let's try the debugging session again. Set the startup project to DebuggingFeatures.Server.
Run. Because Visual Studio is enabling debugging in the browser you will see that is takes 
slightly longer to start.

Now lets open the Counter tab. Click the Increment button again. Your breakpoint should be
hit and Visual Studio's debugger stops at the breakpoint.

Hovering over the CurrentCount property now does not reveal its value!
The CurrenCount property is stored in the currentCount field, which you can inspect
by hovering or dragging it to the watch window. You can also use the Locals window.

Try to set the currentCount field back to 0. This does not work with Blazor WebAssembly.

Now for the FetchData tab. You can hover over the forecasts local variable, and you can 
expand to see the elements of the array.

Now we want to see the day of the week of the first element like with Blazor Server.
When you type the `forecasts[0].Date.DayOfWeek` expression in the watch window you will
see that this does not work with Blazor WebAssembly.

Let's try the buggy loop. Open the Looper tab and click the Loop button. Your application
will crash. The browser's debugger does not support stopping on uncaught exceptions!

## Targeting both Blazor Server andBlazor WebAssembly

We just saw that with Blazor Server we can use every feature of the debugger.
But you need to deliver a Blazor WebAssembly application. Can we combine best of both worlds?

As a famous person once said: Yes! We can!

In this section of the video we will restructure our application to use both Blazor runtimes.

### What will we do?

We will restructure a Blazor WebAssembly project by moving all components into a
component library. We will then add a Blazor Server project using the same component
library and this way allow us to debug our components and other classes with all 
debugging features enabled, and still run everything with Blazor WebAssembly.

This works because we will be using exactly the same components for both runtimes.

### Create the project

Start Visual Studio and create a new hosted Blazor WebAssembly project called Demo.
This will create Demo.Client, Demo.Server and Demo.Shared.

### Add the component library and move the components

Now add a component Library project (Razor Class Library) to the solution. 
Call it `Demo.Components`. Delete the `wwwroot` and other created components from this 
project. You don't need them.

Update the Components project to use `netstandard2.1`. You need this for the following step.

Add a project reference to `Demo.Shared`, because you use classes from the shared project.

Use nuget to add the `System.Net.Http.Json package`. You need this.

Copy `_Imports.razor` from the `WebAssembly` to the `Components` project, 
Rename the usings to `Components`.

Move the Shared and Pages folder with its components to the Component Library
Build the Demo.Components project. This should succeed without errors.

Move App.razor to the Components project.

Update the `AppAssembly` to fix routing. Routing will scan this assembly looking
for @page directives and your components now live in Demo.Components.

```
<Router AppAssembly="@typeof(Pages.Counter).Assembly" >
```

### Use the Components library in WebAssembly

Add a reference in your Blazor WebAssembly project to the Component Library.

In `_Imports.razor` rename the usings to `Components`. This is where your components live now.

Add a using to Program.cs to Demo.Components

```
using DebuggingFeatures.Components;
```

 ### Test your changes

 Run the project. Everything should work as before!

 ### Add the Blazor Server project for debugging

 Right-click your solution and add a new Blazor Server project. Name if Demo.Debugging.

 Delete the Shared folder. From the Pages folder delete Counter, Index and Fetchdata.
 All these live in the Components project, so add a project reference to it.

 Delete WeatherForecast from the Data folder. Add a reference to Demo.Shared.
 Add a using to `WeatherForecastService`.

 In `_Imports.razor` rename the usings to `Components`. This is where your components live now.

 In _Host.cshtml change the typeof to point to the Components's App.
 
 ```
<component type="typeof(Demo.Components.App)" render-mode="ServerPrerendered" />
```

 ### Run the Blazor Server project 

 Build the Demo.Debugging project and run it.

 The Index and Counter component should work like before. Fetchdata we will fix later.

 Put a breakpoint on Counter's IncrementCount'. 
 You can now use the full debugger to debug your components.

 ## Debugging Tips and Tricks

In the previous segment we looked at how you can structure your Blazor application to work for both
Blazor Server and Blazor WebAssembly. Now that we have this, we can look at some advanced debugging tips and tricks!

### Breakpoints

Let's start by looking at how breakpoints are visualized. As you can see here I have a method `Breakpoints`
with on each line a different kind of breakpoint. On the first line a have a simple breakpoint, which you 
can set by clicking the gutter area, or by pressing the **Toggle Breakpoint** key, which is on my machine **F9**.
Repeat to remove the breakpoint again.

When you don't want the debugger to stop on a breakpoint, but you don't want to delete it because you will need it later,
you can disable the breakpoint by right-clicking it and selecting **Disable breakpoint**. Or press **Ctrl-F9**.

You can also create a tracepoint. A tracepoint will write a message in the output window. This is handy is you like to watch
a variable's value without the code stopping all the time. After setting the breakpoint, right-click it and select **Actions**.
Here you can format the message, and select from a series of handy built-in variables such as **$FUNCTION** which gives you the name
of the function, or **$CALLER** which gives you the name of the calling function.

All of these actions can also be done from the Breakpoints window, which you can open from Debug->Windows->Breakpoints.

Both simple breakpoints and tracepoints work in Blazor WebAssembly. 

Let's look at advanced functionality that does not work in Blazor WebAssembly.

A breakpoint can have conditions, where you can make the debugger stop on the breakpoint when certain criteria have been met.
This is especially handy in large loops, where you don't need the breakpoint to stop every time.

Right-click the breakpoint and select conditions. Let's start with the **Conditional Expression** from the dropdown. Enter
an expression that will evaluate to true of false. When the expression evaluates to true, the debugger will stop.

For example, in this breakpoint I have set the condition to `message == null`. This breakpoint will hit when message is null.

You can also set complex expressions, but the debugger will interpret these, making the debugging experience slow.
In that case I recommend using an "Assertion on the fly" where you write a little helper method that evaluates to a bool.

For example, I want the breakpoint to stop when message is null or longer then 10. So I have written a debug helper method:

```
#if DEBUG
    /// <summary>
    /// Debug condition checking if message is not too long, or null
    /// </summary>
    public bool CheckMessage(string message)
    {
      if(message != null)
      {
        return message.Length > 10;
      }
      return true;
    }
#endif
```

Note that this method will only be available in de debug version.

Now we can configure the breakpoint to use this:

In some cases this can still be too slow. So we can use the `Debugger.IsAttached` property to see it the debugger is attached and 
make it stop with the `Debugger.Break()` method.

With Visual Studio you can set breakpoints on a line of code, but you can also set breakpoints on parts of the line. 
For example this can be quite handy with for loops.

Let's look at an example.

I have here a method that will create a list of random elements from the `nrs` collection. 
I would like to see each 10th iteration in this loop.

First of all I am going to add 2 breakpoints. One one the initialization of the loop, and another on the next iteration.
Now, using the breakpoints window I will edit the second breakpoint.

We will use the hitcount feature. This will make the breakpoint stop after a number of hits. You can give it an exact number,
or you can use the multiple feature. This is what I need. I will tell it to stop after each 10th hit.

Now I have another problem. I just wrote this method, and I am calling it from nowhere yet. How can I debug this method?
Using one of the **watch** windows does not help, because this will evaluate the expression outside the debugger.

But with the **immediate** window I can call this method. 
Simple enter an expression that will call the method after setting your breakpoints.
I can now debug this method with ease.

### Summary

Here we looked at the different kinds of breakpoints we can use with Visual Studio.
Developers use simple breakpoints from day to day. But the more advanced breakpoints, 
using conditions and hitcount are the secret to solving your bugs a lot faster.
There are also tracepoints, which allow you to write some value to the output window
which is an easy way to add some tracing.


I have more tips for you, which we will discuss in the building and debugging services segment!

---

## Building and Debugging Services

Remember our Demo application, where we structured our application to support both Blazor Server
and Blazor WebAssembly? The **WeatherForecast** page here is not working as it should. We will solve
this in this chapter by wrapping the logic as a service, and then make it available both in
Blazor Server and WebAssembly.

### Create the WeatherForecast Service

Start by looking at the FetchData component. This uses the HttpClient to download forecast
data from the server. This does not work for the Blazor Server project, because we don't have
a REST endpoint listening for this request. We don't need this anyway, because we are already 
on the server!!!

What we need to do is to encapsulate this as a service. In Blazor Server we can use this service
directly, and on Blazor WebAssembly we will use a proxy to access the service.

Start by adding the `IWeatherService` interface to the Shared project. 

``` csharp
public interface IWeatherService
{
  ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate);
}
```

Implement this interface as the `WeatherForecastService` class. Since it only has a dependency on
classes from the Shared project we can add this to the Shared project:

``` csharp
public class WeatherForecastService : IWeatherService
{
  private static readonly string[] Summaries = new[]
  {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  public async ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate)
  {
    var rng = new Random();
    return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = startDate.AddDays(index),
      TemperatureC = rng.Next(-20, 55),
      Summary = Summaries[rng.Next(Summaries.Length)]
    }).ToArray());
  }
}
```

Do note that testing this service can be done outsite of Blazor, using unit tests or whatever you prefer.

### Consume the WeatherForecastService in Blazor Server

Open the `FetchData.razor` component, and replace the dependency from ``HttPClient` to `IWeatherService`:

``` csharp
@inject IWeatherService weatherService
```

Replace the code to get the forecasts:

``` csharp
@code {
    private IEnumerable<WeatherForecast> forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await weatherService.GetAsync(DateTime.Now);
    }
}
```

Now we are almost ready to test this with Blazor Server. We just need to configure
dependency injection.

Start by adding a new static class to the shared project which hides the exact dependency using an extension method:

``` csharp
public static class DependencyInjection
{
  public static IServiceCollection AddWeatherServicesForServer(this IServiceCollection services)
  {
    services.AddTransient<IWeatherService, WeatherForecastService>();
    return services;
  }
}
```

Then in the Blazor Server project replace the dependency:

``` csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddRazorPages();
  services.AddServerSideBlazor();
  services.AddWeatherServicesForServer();
}
```

Run and test your Blazor Server project. This should work. Don't forget that here the 
FetchData talks directly to the code that retrieves the data. No need to pass over the network!

### Consume the WeatherForecastService in Blazor WebAssembly

Let us fix the Blazor WebAssembly project. The FetchData component needs to fetch the data over the network.
We will implement the `IWeatherService` to fetch the data over the network using REST.
Let us start by implementing the controller to expose a REST endpoint so our WebAssembly project can access it.

Update the `WeatherForecastController` controller to use the service:

``` csharp
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
    public async Task<IEnumerable<WeatherForecast>> Get() => await this.weatherService.GetAsync(DateTime.Now);
  }
}
```

Do note that the IWeatherService implementation is reused in the Blazor Component for Blazor Server, and for 
the controller in Blazor WebAssembly.

Now we can create an implementation of the `IWeatherService` that accesses the forecast using a REST call:

Add the **System.Net.Http.Json** NuGet package to the Shared project

```
<PackageReference Include="System.Net.Http.Json" Version="3.2.0" />
```

We can now create the `WeatherForecastProxy` service implementation:

``` csharp
namespace Demo.Shared
{
  public class WeatherForecastProxy : IWeatherService
  {
    private readonly HttpClient httpClient;

    public WeatherForecastProxy(HttpClient httpClient)
    {
      this.httpClient = httpClient;
    }

    public async ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate)
    {
      return await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("weatherforecast");
    }
  }
}
```

The idea is to make the Blazor WebAssembly use the proxy to retrieve forecasts. For this we will add
the proxy using Dependency Injection:

``` csharp
public static IServiceCollection AddWeatherServicesForWebAssembly(this IServiceCollection services)
{
  services.AddTransient<IWeatherService, WeatherForecastProxy>();
  return services;
}
```

To finish it all, call this method in your Blazor WebAssembly `Program.cs`:

``` csharp
namespace Demo.Client
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("app");

      builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      builder.Services.AddWeatherServicesForWebAssembly();

      await builder.Build().RunAsync();
    }
  }
}
```

You should now be able to run the Blazor WebAssembly version, and retrieve the forecasts.

Lets have a look at an illustration to show what we just did.

### More Debugging Tips

#### DebuggerDisplay attributes

Let's put a breakpoint on the `OnInitializedAsync` method of the `FetchData` component.

Services typically return collections of objects. Inspecting these objects in the debugger can become
quit hard, since you have to open the collection to see the instances, and then you have to expand
each instance to examine its contents.

What is you could customize how the debugger displays the items of a collection? 
Using the `System.Diagnostics` namespace you can add attributes in your code to tell the debugger 
how to display instances, and how you want to step over and into your methods?

For example you can add the `DebuggerDisplay` attribute like this:

``` csharp
[DebuggerDisplay("{Date.DayOfWeek} {Date,nq} {TemperatureC}")]
```

Now when you expand the collection you instantly see the information you need.

#### Hitcount breakpoints

``` csharp
  protected override async Task OnInitializedAsync()
  {
    forecasts = await weatherService.GetAsync(DateTime.Now);

    foreach (var fc in forecasts)
    {
      try
      {
        forecastProcessor.Process(fc);
      }
      catch (Exception ex)
      {
          string msg = ex.Message;
      }
    }
  }
```

Suppose some method crashes from time to time. Then the hitcount feature can also be a big help.
I would like to have a breakpoint that will let me see the steps right before the crash. But it is a big loop.
I don't want to have to step a 1000 times before I see the crash!

Let's click on DoTheLoop method. Is crashes somewhere in the iteration...
Which iteration does it crash? 

Assume there is nothing in this method that allows you to easily see which iteration you're in.

We can easily figure this out using the hitcount feature.
Add a breakpoint in this method, and set the hitcount to some large number, eg. 100000
Now run again. It crashes again. But now look in the breakpoint window. This will give you the hitcount
when the iteration crashed. Update the breakpoint to use this hitcount (345).
This will stop on the breakpoint, right before the crash!

You can also add something called a data breakpoint. Here we have an instance of `SomeClass` and 
somewhere in the code this instance gets updated. How can we figure out where this update occurs?

First we need to get a reference to this object. Here I can easily do this using the **Locals** window.
Expand `this`, then expand `someClass`. 
Right-click the `Message` reference and select **Break when value changes**.

Now, if somewhere this reference's `Message` gets changed, the debugger will break.

Did you see this `$1` in the Watch window? This is know as an **Object ID**. This this you 
can tell the debugger to keep a reference to an instance so you can look at it at any time.
This will allow you to access an instance, even in code that normally cannot see that instance.

Let's go to the FetchData page. In the `OnInitialized` it loads forecasts from the server.
I would like to play with this a little to see how the rendering holds. So I have added a demo
button.

But first I need a reference to the first forecast. I will use an object ID for this.
Expand `this`, then `forecasts`. Select the first element and **Make Object ID**.
Continue running. Now click the **Demo** button and change the temperature of the first element.
This way we can easily "drive" our controls and see how they handle different kinds of data.

We also looked at objectID's which allow you to examine any instance at any point of time,
easily reachable using the Locals or Watch window.

## Logging

Some bugs only occur under specific circumstances, and in order to fix a bug like this you 
first need to reproduce it. In that case logging may be your last resort to find the right
environment in which to reproduce that hard-to-catch bug.

https://blog.stephencleary.com/2018/06/microsoft-extensions-logging-part-2-types.html 
https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-3.1


### Logging Introduction

Let us start with a little introduction to Logging.

Blazor uses the same logging infrastructure as ASP.NET Core, the `ILogger<T>` interface.
The generic `ILogger<T>` is just a handy way to specify the **Category** to be used by
the logger. With the **Category** you can filter your log files to look for the component or service
that did the logging. For example `ILogger<Counter>` will use the "Counter" category for logging.
The generic interface will use the `FullName` of the Type passed as the Category.

Logging also uses a `LogLevel` enumeration, which gives you control over which messages get logged,
and which get thrown away. You do this by specifying the minimum logging level when you setup logging.

**Slide about logging levels with their meaning**

For example, when you set the minumum level to **Information**, messages with loglevel **Error** will
be logged, while messages with loglevel **Debug** and **Trace** will be thrown away.

Let's look at the `ILogger` interface:

```
public interface ILogger
{
    void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

    bool IsEnabled(LogLevel logLevel);

    IDisposable BeginScope<TState>(TState state);
}
```

The main method of this interface is the `Log` method. For your convenience there are several extension methods such
as `LogInformation` which make calling this method more convenient, so I advice you use these.

Start by adding the `Microsoft.Extensions.Logging` namespace to `_Imports.razor`.

```
@using Microsoft.Extensions.Logging 
```

Now you can use **Dependency Injection** to get an instance of the `ILogger<T>` interface in your component,
for example with

```
@inject ILogger<Counter> logger
```

Of course you can also use the `[Inject]` attribute if you use code seperation, or use a service's constructor.

Once you have the instance, you can call the Log methods on it, for example in the `Counter` component:

```
private void IncrementCount()
{
  currentCount++;
  logger.LogInformation("Counter incremented to {currentCount}", currentCount);
}
```

### Selecting the logger

With Blazor Server, logging is automatically enabled for you, so you can hit **F5** to run.

By default the logging will be sent to the Console and to Visual Studio's output window.

You can set the minimum loglevel using configuration. Open your server project's `appsettings.json`
and specify the minimum loglevel for your logger's Category. For example "Demo.Components.Pages.Counter":

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Demo.Components.Pages.Counter": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

**Show output window**

With Blazor WebAssembly logging will send the log messages to the Browser's Console.

**Show Browsers Console window**

You can specify the minimum loglevel using the `SetMinimumLevel` method in your `Program.cs`.
For example:

```
public static async Task Main(string[] args)
{
  var builder = WebAssemblyHostBuilder.CreateDefault(args);
  builder.RootComponents.Add<App>("app");

  builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

  builder.Services.AddWeatherServiceForWebAssembly();

  builder.Logging.SetMinimumLevel(LogLevel.Information);

  await builder.Build().RunAsync();
}
```

You can also use a configuration file to configure logging. But with Blazor WebAssembly you will need to do
some extra work.

Start by adding the **Microsoft.Extensions.Logging.Configuration** package to your Blazor WebAssembly project.

Now add an `appsettings.json` file to the `wwwroot` folder of your project. Of simply copy the file
from the Blazor Server project.

Now add a call to `builder.Logging.AddConfiguration` to `program.cs`.

```
public static async Task Main(string[] args)
{
  var builder = WebAssemblyHostBuilder.CreateDefault(args);
  builder.RootComponents.Add<App>("app");

  builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

  builder.Services.AddWeatherServiceForWebAssembly();

  // builder.Logging.SetMinimumLevel(LogLevel.Error);

  builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

  await builder.Build().RunAsync();
}
```

### Add a guard before logging

Sometimes you need to fetch some data to make it part of the logging message.
When that data is expensive at runtime, you might want to skip fetching it when you know logging is 
going to throw away the logging because of the minumum loglevel.

With the `logger.IsEnabled` method you can check if logging will actually occur.
Look at this example:


```
logger.LogInformation("Counter incremented to {currentCount}", currentCount);
```

Pretend that fetching `currentCount` takes some time. Then you can use an if statement
to check the minimum logLevel:

```
if (logger.IsEnabled(LogLevel.Information))
{
  logger.LogInformation("Counter incremented to {currentCount}", currentCount);
}
```

### Adding 'Breadcrumbs" for logging

Sometimes seeing the call chain can make your logging a lot more practical. For this logging
allows you to create scopes. Best way to understand this is again with an example.

The `FetchData` component uses the `WeatherForecastService` service to fetch the data.

To use scopes you first need to enable scopes using configuration:

```
"Console": {
  "IncludeScopes": true, // Required to use Scopes.
  "LogLevel": {
    "Microsoft": "Warning",
    "Default": "Information",
    "Demo.Components.Pages.FetchData": "Information"
  }
}
```

Next, inside the FetchData component we create a scope:

```
    protected override async Task OnInitializedAsync()
    {
      using (var scope = logger.BeginScope("Inside FetchData"))
      {
        try
        {
          logger.LogInformation("Fetching weatherdata");
          forecasts = await weatherService.GetAsync(DateTime.Now);
          logger.LogInformation("Fetch successfull");
        }
        catch(Exception ex)
        {
          logger.LogError(ex, "Failed featching weatherdata");
        }
      }
    }
```

To enable logging in the `WeatherForecastService` service, we need to add a package reference:

```
<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="3.1.5" />
```

Next we add a constructor to the `WeatherForecastService`

```
    private readonly ILogger<WeatherForecast> logger;
    public WeatherForecastService(ILogger<WeatherForecast> logger)
    {
      this.logger = logger;
    }
```

And we create another scope inside the `GetAsync` method

```
public async ValueTask<IEnumerable<WeatherForecast>> GetAsync(DateTime startDate)
{
  using (var scope = logger.BeginScope("Inside WeatherForecastService"))
  {
    logger.LogInformation("Getting forecast for {startDate}", startDate);
    var rng = new Random(101);
    return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = startDate.AddDays(index),
      TemperatureC = rng.Next(-20, 55),
      Summary = Summaries[rng.Next(Summaries.Length)]
    }).ToArray());
  }
}
```
























