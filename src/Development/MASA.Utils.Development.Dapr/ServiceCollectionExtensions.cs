namespace MASA.Utils.Development.Dapr;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDapr(this IServiceCollection services, Action<DaprOptions> options)
    {
        return services;
    }
}
