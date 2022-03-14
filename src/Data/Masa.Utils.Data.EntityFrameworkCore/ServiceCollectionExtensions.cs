namespace Masa.Utils.Data.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasaDbContext<TDbContext>(
        this IServiceCollection services,
        Action<MasaDbContextOptionsBuilder>? options = null)
        where TDbContext : MasaDbContext
    {
        var builder = new MasaDbContextOptionsBuilder<TDbContext>(services);
        options?.Invoke(builder);

        services.AddDbContext<TDbContext>(builder.ContextLifetime, builder.OptionsLifetime);

        return services
            .TryAdd(typeof(MasaDbContextOptions<TDbContext>),
                serviceProvider => CreateMasaDbContextOptions<TDbContext>(serviceProvider, builder.Options), builder.OptionsLifetime)
            .TryAdd(typeof(MasaDbContextOptions), serviceProvider => serviceProvider.GetRequiredService<MasaDbContextOptions<TDbContext>>(),
                builder.OptionsLifetime);
    }

    private static IServiceCollection TryAdd(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory,
        ServiceLifetime lifetime)
    {
        var serviceDescriptor = new ServiceDescriptor(serviceType, factory, lifetime);
        services.TryAdd(serviceDescriptor);
        return services;
    }

    private static MasaDbContextOptions<TDbContext> CreateMasaDbContextOptions<TDbContext>(IServiceProvider serviceProvider,
        DbContextOptions options) where TDbContext : MasaDbContext => new(options, serviceProvider);
}
