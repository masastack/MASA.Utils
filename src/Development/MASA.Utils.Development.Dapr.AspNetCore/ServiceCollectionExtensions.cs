namespace MASA.Utils.Development.Dapr.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprStarter(
        this IServiceCollection services,
        Action<DaprOptions>? action = null,
        Action<DaprBackgroundOptions>? daprBackgroundOptionsAction = null)
    {
        DaprOptions daprOptions = new();
        action?.Invoke(daprOptions);
        return services.AddDaprStarter(() => daprOptions, daprBackgroundOptionsAction);
    }

    public static IServiceCollection AddDaprStarter(this IServiceCollection services,
        Func<DaprOptions> func,
        Action<DaprBackgroundOptions>? action = null)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
        {
            return services;
        }
        services.AddSingleton<DaprService>();

        if (action != null)
            services.Configure(action);

        services.AddHostedService<DaprBackgroundService>();
        services.Configure<HostOptions>(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(15));
        return services.AddDaprStarterCore(func);
    }

    public static IServiceCollection AddDaprStarter(this IServiceCollection services, IConfiguration configuration)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
        {
            return services;
        }
        services.AddSingleton<DaprService>();
        services.AddHostedService<DaprBackgroundService>();
        services.Configure<DaprBackgroundOptions>(configuration);
        return services.AddDaprStarterCore(configuration);
    }

    private class DaprService
    {

    }
}
