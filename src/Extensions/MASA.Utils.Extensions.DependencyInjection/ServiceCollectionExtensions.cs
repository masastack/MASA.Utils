namespace Microsoft.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <param name="services"></param>
    /// <param name="suffix">default is Service</param>
    public static IServiceCollection AddServices(this IServiceCollection services, string suffix, bool autoFire)
        => services.AddServices(suffix, autoFire, Assembly.GetEntryAssembly()!);

    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <param name="services"></param>
    /// <param name="suffix">default is Service</param>
    /// <param name="autoFire"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services, string suffix, bool autoFire, params Assembly[] assemblies)
        => (from type in assemblies.SelectMany(assembly => assembly.GetTypes())
            where type.Name.EndsWith(suffix)
            select type).AddScoped(services, autoFire);

    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="services"></param>
    /// <param name="autoFire"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices<TService>(this IServiceCollection services, bool autoFire)
        => services.AddServices<TService>(autoFire, new Assembly[1]
        {
            Assembly.GetEntryAssembly()!
        });

    /// <summary>
    /// Auto add all service to IoC, lifecycle is scoped
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="services"></param>
    /// <param name="autoFire"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices<TService>(this IServiceCollection services, bool autoFire, params Assembly[] assemblies)
        => (from type in assemblies.SelectMany(assembly => assembly.GetTypes())
            where type.BaseType == typeof(TService)
            select type).AddScoped(services, autoFire);

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
