namespace Masa.Utils.Ldap.Extensions;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddLadpContext(this IServiceCollection services, Action<LdapOptions> optionsAction)
    {
        services.Configure(optionsAction);
        services.AddSingleton(typeof(ILdapProvider), typeof(LdapProvider));
        return services;
    }

    public static IServiceCollection AddLadpContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LdapOptions>(configuration);
        services.AddSingleton(typeof(ILdapProvider), typeof(LdapProvider));
        return services;
    }
}
