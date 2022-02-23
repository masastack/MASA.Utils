namespace Masa.Utils.Security.Token;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services, Action<JwtConfigurationOptions> options)
    {
        services.Configure(options);
        services.TryAddScoped<IJwtProvider, DefaultJwtProvider>();
        new JwtUtils(services);
        return services;
    }
}
