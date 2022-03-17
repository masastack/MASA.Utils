using Microsoft.Extensions.Configuration;

namespace Masa.Utils.Data.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasaDbContext<TDbContext>(
        this IServiceCollection services,
        Action<MasaDbContextOptionsBuilder>? optionsAction = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        where TDbContext : MasaDbContext
        => services.AddMasaDbContext<TDbContext>(
            (_, masaDbContextOptionsBuilder) => optionsAction?.Invoke(masaDbContextOptionsBuilder),
            contextLifetime,
            optionsLifetime);

    public static IServiceCollection AddMasaDbContext<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, MasaDbContextOptionsBuilder>? optionsAction = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        where TDbContext : MasaDbContext
        => services
            .AddDbContext<TDbContext>(contextLifetime, optionsLifetime)
            .AddCoreServices<TDbContext>(optionsAction, optionsLifetime);

    private static IServiceCollection AddCoreServices<TDbContextImplementation>(
        this IServiceCollection services,
        Action<IServiceProvider, MasaDbContextOptionsBuilder>? optionsAction,
        ServiceLifetime optionsLifetime)
        where TDbContextImplementation : MasaDbContext
    {
        services.TryAddConfigure<MasaDbConnectionOptions>();
        services.TryAddScoped<IConnectionStringProvider, DefaultConnectionStringProvider>();
        services.TryAddScoped(typeof(DataFilter<>));
        services.TryAddScoped<IDataFilter, DataFilter>();
        services.TryAddEnumerable(new ServiceDescriptor(typeof(ISaveChangesFilter), typeof(SoftDeleteSaveChangesFilter),
            ServiceLifetime.Scoped));

        services.TryAdd(
            new ServiceDescriptor(
                typeof(MasaDbContextOptions<TDbContextImplementation>),
                serviceProvider => CreateMasaDbContextOptions<TDbContextImplementation>(serviceProvider, optionsAction),
                optionsLifetime));

        services.Add(
            new ServiceDescriptor(
                typeof(MasaDbContextOptions),
                serviceProvider => serviceProvider.GetRequiredService<MasaDbContextOptions<TDbContextImplementation>>(),
                optionsLifetime));
        return services;
    }

    private static MasaDbContextOptions<TDbContext> CreateMasaDbContextOptions<TDbContext>(
        IServiceProvider serviceProvider,
        Action<IServiceProvider, MasaDbContextOptionsBuilder>? optionsAction)
        where TDbContext : MasaDbContext
    {
        var masaDbContextOptionsBuilder = new MasaDbContextOptionsBuilder<TDbContext>(serviceProvider);
        optionsAction?.Invoke(serviceProvider, masaDbContextOptionsBuilder);

        return CreateMasaDbContextOptions<TDbContext>(
            serviceProvider,
            masaDbContextOptionsBuilder.DbContextOptionsBuilder.Options,
            masaDbContextOptionsBuilder.EnableSoftwareDelete);
    }

    private static MasaDbContextOptions<TDbContext> CreateMasaDbContextOptions<TDbContext>(IServiceProvider serviceProvider,
        DbContextOptions options, bool enableSoftware) where TDbContext : MasaDbContext => new(serviceProvider, options, enableSoftware);

    private static IServiceCollection TryAddConfigure<TOptions>(
        this IServiceCollection services)
        where TOptions : class
        => services.TryAddConfigure<TOptions>(Const.DefaultSection);

    private static IServiceCollection TryAddConfigure<TOptions>(
        this IServiceCollection services,
        string sectionName)
        where TOptions : class
    {
        IConfiguration? configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        if (configuration == null)
            return services;

        string name = typeof(TOptions).FullName ?? typeof(TOptions).Name;
        services.AddOptions();
        var configurationSection = configuration.GetSection(sectionName);
        services.TryAddSingleton<IOptionsChangeTokenSource<TOptions>>(
            new ConfigurationChangeTokenSource<TOptions>(name, configurationSection));
        services.TryAddSingleton<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name,
            configurationSection, _ =>
            {
            }));
        return services;
    }
}
