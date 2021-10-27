namespace MASA.Utils.Caching.DistributedMemory.Interfaces;

/// <summary>
/// A factory abstraction for a component that create <see cref="IMemoryCacheClient"/> instances with custom configuration for a given logical name.
/// </summary>
public interface IMemoryCacheClientFactory : ICacheClientFactory<MemoryCacheClient>
{
}
