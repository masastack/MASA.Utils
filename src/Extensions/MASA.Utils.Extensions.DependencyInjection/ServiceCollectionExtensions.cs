using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <param name="services"></param>
    /// <param name="suffix">default is Service</param>
    public static IServiceCollection AddServices(this IServiceCollection services, string suffix, bool autoFire)
    {
        return Assembly
            .GetEntryAssembly()!
            .GetTypes()
            .Where(t => t.Name.EndsWith(suffix))
            .AddScoped(services, autoFire);
    }

    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <param name="services"></param>
    /// <param name="suffix">default is Service</param>
    public static IServiceCollection AddServices<TService>(this IServiceCollection services, bool autoFire)
    {
        var serviceType = typeof(TService);

        return Assembly
            .GetEntryAssembly()!
            .GetTypes()
            .Where(t => t.BaseType == serviceType)
            .AddScoped(services, autoFire);
    }

    private static IServiceCollection AddScoped(this IEnumerable<Type> serviceTypes, IServiceCollection services, bool autoFire)
    {
        foreach (var serviceType in serviceTypes)
        {
            services.AddScoped(serviceType);
        }

        if (autoFire)
        {
            foreach (var serviceType in serviceTypes)
            {
                services.BuildServiceProvider().GetService(serviceType);
            }
        }
        return services;
    }
}
