using DebuggingFeatures.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DebuggingFeatures.Components
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
      return services.AddScoped<ILocalStorage, LocalStorage>();
    }
  }
}
