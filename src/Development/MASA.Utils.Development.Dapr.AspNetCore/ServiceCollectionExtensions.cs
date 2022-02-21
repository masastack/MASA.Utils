﻿namespace MASA.Utils.Development.Dapr.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDapr(
        this IServiceCollection services,
        Action<DaprOptions>? action = null,
        Action<DaprBackgroundOptions>? daprBackgroundOptionsAction = null)
    {
        DaprOptions daprOptions = new();
        action?.Invoke(daprOptions);
        return services.AddDapr(() => daprOptions, daprBackgroundOptionsAction);
    }

    public static IServiceCollection AddDapr(this IServiceCollection services,
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
        return services.AddDaprCore(func);
    }

    public static IServiceCollection AddDapr(this IServiceCollection services, IConfiguration configuration)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
        {
            return services;
        }
        services.AddSingleton<DaprService>();
        services.AddHostedService<DaprBackgroundService>();
        services.Configure<DaprBackgroundOptions>(configuration);
        return services.AddDaprCore(configuration);
    }

    private class DaprService
    {

    }
}
