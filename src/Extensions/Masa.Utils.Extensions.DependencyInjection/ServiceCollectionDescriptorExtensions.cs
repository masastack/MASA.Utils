// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionDescriptorExtensions
{
    public static TService GetInstance<TService>(this IServiceCollection services) where TService : class
        => services.BuildServiceProvider().GetRequiredService<TService>();

    public static bool Any<TService>(this IServiceCollection services) where TService : class
        => services.Any(d => d.ServiceType == typeof(TService));

    public static bool Any<TService>(this IServiceCollection services, ServiceLifetime lifetime) where TService : class
        => services.Any(d => d.ServiceType == typeof(TService) && d.Lifetime == lifetime);
}
