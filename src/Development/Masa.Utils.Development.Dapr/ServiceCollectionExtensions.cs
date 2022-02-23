namespace Masa.Utils.Development.Dapr;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprStarterCore(this IServiceCollection services, Action<DaprOptions>? action = null)
    {
        DaprOptions daprOptions = new();
        action?.Invoke(daprOptions);
        return services.AddDaprStarterCore(() => daprOptions);
    }

    public static IServiceCollection AddDaprStarterCore(this IServiceCollection services, Func<DaprOptions> func)
    {
        services.AddSingleton(Options.Create(func.Invoke()));
        return services.AddDaprStarterCore();
    }

    public static IServiceCollection AddDaprStarterCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DaprOptions>(configuration);
        return services.AddDaprStarterCore();
    }

    private static IServiceCollection AddDaprStarterCore(this IServiceCollection services)
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
