namespace Masa.Utils.Development.Dapr.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprStarter(
        this IServiceCollection services,
        Action<DaprOptions>? daprOptionAction = null)
    {
        return services.AddDaprStarter(opt =>
        {
            daprOptionAction?.Invoke(opt);
        }, _ => { });
    }

    public static IServiceCollection AddDaprStarter(this IServiceCollection services,
        Action<DaprOptions> daprOptionAction,
        Action<DaprBackgroundOptions> action)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
            return services;
        services.AddSingleton<DaprService>();

        services.Configure(action);

        services.AddHostedService<DaprBackgroundService>();
        services.Configure<HostOptions>(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(15));
        return services.AddDaprStarterCore(daprOptionAction);
    }

    public static IServiceCollection AddDaprStarter(this IServiceCollection services, IConfiguration configuration)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
            return services;
        services.AddSingleton<DaprService>();

        services.AddHostedService<DaprBackgroundService>();
        services.Configure<DaprBackgroundOptions>(configuration);
        return services.AddDaprStarterCore(configuration);
    }

    private class DaprService
    {

    }
}
