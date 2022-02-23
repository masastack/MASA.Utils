namespace Masa.Utils.Caching.Redis.DependencyInjection;

/// <summary>
/// Extensions methods to configure an <see cref="IServiceCollection"/> for <see cref="IDistributedCacheClientFactory"/>.
/// </summary>
public static class RedisCacheClientFactoryServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IDistributedCacheClientFactory"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureOptions">A delegate that is used to configure an <see cref="IDistributedCacheClient"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static ICachingBuilder AddMasaRedisCache(this IServiceCollection services, Action<RedisConfigurationOptions> configureOptions)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.TryAddSingleton<IDistributedCacheClientFactory, RedisCacheClientFactory>();

        services.TryAddSingleton(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IDistributedCacheClientFactory>();
            return factory.CreateClient(string.Empty);
        });

        var builder = new CachingBuilder(services, string.Empty);

        builder.ConfigureDistributedCacheClient(configureOptions);

        return builder;
    }

    /// <summary>
    /// Adds the <see cref="IDistributedCacheClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures a named <see cref="IDistributedCacheClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="name">The logical name of the <see cref="IDistributedCacheClient"/> to configure.</param>
    /// <param name="configureOptions">A delegate that is used to configure an <see cref="IDistributedCacheClient"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static ICachingBuilder AddMasaRedisCache(this IServiceCollection services, string name, Action<RedisConfigurationOptions> configureOptions)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.TryAddSingleton<IDistributedCacheClientFactory, RedisCacheClientFactory>();

        var builder = new CachingBuilder(services, name);

        builder.ConfigureDistributedCacheClient(configureOptions);

        return builder;
    }

    /// <summary>
    /// Adds the <see cref="IDistributedCacheClientFactory"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="options">The <see cref="RedisConfigurationOptions"/> to configure an <see cref="IDistributedCacheClient"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static ICachingBuilder AddMasaRedisCache(this IServiceCollection services, RedisConfigurationOptions options)
    {
        return services.AddMasaRedisCache(o => o.Initialize(options));
    }

    /// <summary>
    /// Adds the <see cref="IDistributedCacheClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures a named <see cref="IDistributedCacheClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="name">The logical name of the <see cref="IDistributedCacheClient"/> to configure.</param>
    /// <param name="options">The <see cref="RedisConfigurationOptions"/> to configure an <see cref="IDistributedCacheClient"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static ICachingBuilder AddMasaRedisCache(this IServiceCollection services, string name, RedisConfigurationOptions options)
    {
        return services.AddMasaRedisCache(name, o => o.Initialize(options));
    }
}
