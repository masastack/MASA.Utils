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
        Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(t => t.Name.EndsWith(suffix))
            .ToList()
            .ForEach(serviceType => AddScoped(services, serviceType, autoFire));

        return services;
    }

    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <param name="services"></param>
    /// <param name="suffix">default is Service</param>
    public static IServiceCollection AddServices<TService>(this IServiceCollection services, bool autoFire)
    {
        var serviceType = typeof(TService);

        Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(t => t.BaseType == serviceType)
            .ToList()
            .ForEach(serviceType => AddScoped(services, serviceType, autoFire));

        return services;
    }

    private static void AddScoped(IServiceCollection services, Type serviceType, bool autoFire)
    {
        services.AddScoped(serviceType);

        if (autoFire) services.BuildServiceProvider().GetService(serviceType);
    }
}
