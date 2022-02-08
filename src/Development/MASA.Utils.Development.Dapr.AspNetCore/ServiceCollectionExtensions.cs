namespace MASA.Utils.Development.Dapr.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDapr(this IServiceCollection services, Action<DaprOptions>? action = null)
    {
        DaprOptions daprOptions = new();
        action?.Invoke(daprOptions);
        return services.AddDapr(() => daprOptions);
    }

    public static IServiceCollection AddDapr(this IServiceCollection services, Func<DaprOptions> func)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
        {
            return services;
        }
        services.AddSingleton<DaprService>();

        services.AddHostedService<DaprBackgroundService>();
        return services.AddDaprCore(func);
    }

    private class DaprService
    {

    }
}
