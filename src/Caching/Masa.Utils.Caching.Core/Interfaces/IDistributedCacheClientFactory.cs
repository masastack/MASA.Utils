namespace Masa.Utils.Caching.Core.Interfaces;

/// <summary>
/// A factory abstraction for a component that create <see cref="IDistributedCacheClient"/> instances with custom configuration for a given logical name.
/// </summary>
public interface IDistributedCacheClientFactory : ICacheClientFactory<IDistributedCacheClient>
{
}
