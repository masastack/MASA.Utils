namespace MASA.Utils.Development.Dapr;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaprCore(this IServiceCollection services, Action<DaprOptions>? action = null)
    {
        DaprOptions daprOptions = new();
        action?.Invoke(daprOptions);
        return services.AddDaprCore(() => daprOptions);
    }

    public static IServiceCollection AddDaprCore(this IServiceCollection services, Func<DaprOptions> func)
    {
        if (services.Any(service => service.ImplementationType == typeof(DaprService)))
        {
            return services;
        }
        services.AddSingleton<DaprService>();

        DaprOptions options = func.Invoke();
        services.TryAddSingleton(typeof(IDaprProcess), typeof(DaprProcess));
        services.TryAddSingleton(typeof(IDaprProvider), typeof(DaprProvider));
        services.TryAddSingleton(typeof(IProcessProvider), typeof(ProcessProvider));
        services.AddSingleton(Options.Create(options));
        return services;
    }

    private class DaprService
    {

    }
}
