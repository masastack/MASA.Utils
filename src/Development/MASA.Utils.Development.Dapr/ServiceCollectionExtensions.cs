namespace MASA.Utils.Development.Dapr;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprStartCore(this IServiceCollection services, Action<DaprOptions>? action = null)
    {
        DaprOptions daprOptions = new();
        action?.Invoke(daprOptions);
        return services.AddDaprStartCore(() => daprOptions);
    }

    public static IServiceCollection AddDaprStartCore(this IServiceCollection services, Func<DaprOptions> func)
    {
        services.AddSingleton(Options.Create(func.Invoke()));
        return services.AddDaprStartCore();
    }

    public static IServiceCollection AddDaprStartCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DaprOptions>(configuration);
        return services.AddDaprStartCore();
    }

    private static IServiceCollection AddDaprStartCore(this IServiceCollection services)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
        {
            return services;
        }
        services.AddSingleton<DaprService>();

        services.TryAddSingleton(typeof(IDaprProcess), typeof(DaprProcess));
        services.TryAddSingleton(typeof(IDaprProvider), typeof(DaprProvider));
        services.TryAddSingleton(typeof(IProcessProvider), typeof(ProcessProvider));
        return services;
    }

    private class DaprService
    {

    }
}
