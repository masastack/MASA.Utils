// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionDescriptorExtensions
{
    public static TService GetInstance<TService>(this IServiceCollection services, bool isCreateScope = false)
        where TService : class
    {
        if (isCreateScope)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            return scope.ServiceProvider.GetInstance<TService>();
        }

        return services.BuildServiceProvider().GetInstance<TService>();
    }

    private static TService GetInstance<TService>(this IServiceProvider serviceProvider)
        where TService : class
    {
        if (typeof(TService) == typeof(IServiceProvider))
            return (TService)serviceProvider;

        return serviceProvider.GetRequiredService<TService>();
    }

    public static bool Any<TService>(this IServiceCollection services) where TService : class
        => services.Any(d => d.ServiceType == typeof(TService));

    public static bool Any<TService>(this IServiceCollection services, ServiceLifetime lifetime) where TService : class
        => services.Any(d => d.ServiceType == typeof(TService) && d.Lifetime == lifetime);
}
