namespace Masa.Utils.Caching.Core.DependencyInjection;

/// <summary>
/// Extension methods for configuring an <see cref="ICachingBuilder"/>
/// </summary>
public static class DistributedCacheClientBuilderExtensions
{
    /// <summary>
    /// Adds a delegate that will be used to configure a named <see cref="IDistributedCacheClient"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ICachingBuilder"/>.</param>
    /// <param name="configureOptions">A delegate that is used to configure an <see cref="IDistributedCacheClient"/>.</param>
    /// <returns>An <see cref="ICachingBuilder"/> that can be used to configure the client.</returns>
    public static ICachingBuilder ConfigureDistributedCacheClient<TOptions>(this ICachingBuilder builder, Action<TOptions> configureOptions) where TOptions : class
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        builder.Services.Configure(builder.Name, configureOptions);

        return builder;
    }
}
