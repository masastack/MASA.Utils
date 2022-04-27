// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Utils.Development.Dapr.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprStarter(this IServiceCollection services)
        => services.AddDaprStarter(_ => { });

    public static IServiceCollection AddDaprStarter(this IServiceCollection services, Action<DaprOptions> daprOptionAction)
    {
        ArgumentNullException.ThrowIfNull(daprOptionAction,nameof(daprOptionAction));

        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
            return services;
        services.AddSingleton<DaprService>();

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
        return services.AddDaprStarterCore(configuration);
    }

    private class DaprService
    {

    }
}
