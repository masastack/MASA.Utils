namespace MASA.Utils.Data.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasaDbContext<TDbContext>(
        this IServiceCollection services,
        Action<MasaDbContextOptionsBuilder>? options = null)
        where TDbContext : MasaDbContext
    {
        var builder = new MasaDbContextOptionsBuilder<TDbContext>(services);
        options?.Invoke(builder);

        services.AddDbContext<TDbContext>();

        services.TryAddScoped(typeof(MasaDbContextOptions<TDbContext>), serviceProvider => CreateMasaDbContextOptions<TDbContext>(serviceProvider, builder.Options));
        services.TryAddScoped<MasaDbContextOptions>(serviceProvider => serviceProvider.GetRequiredService<MasaDbContextOptions<TDbContext>>());

        return services;
    }

    private static MasaDbContextOptions<TDbContext> CreateMasaDbContextOptions<TDbContext>(
        IServiceProvider serviceProvider,
        DbContextOptions options)
        where TDbContext : MasaDbContext
    {
        var queryFilterProviders = serviceProvider.GetServices<IQueryFilterProvider>();
        var saveChangesFilters = serviceProvider.GetServices<ISaveChangesFilter>();
        return new MasaDbContextOptions<TDbContext>(options, queryFilterProviders, saveChangesFilters);
    }
}
