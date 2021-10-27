namespace MASA.Utils.Caching.DistributedMemory;

public class MemoryCacheClientFactory : IMemoryCacheClientFactory
{
    private readonly IServiceProvider _services;

    private readonly IOptionsMonitor<MasaMemoryCacheOptions> _optionsMonitor;

    private readonly ConcurrentDictionary<string, Lazy<MemoryCacheClient>> _clients;

    private readonly Func<string, Lazy<MemoryCacheClient>> _clientFactory;

    public MemoryCacheClientFactory(IServiceProvider services, IOptionsMonitor<MasaMemoryCacheOptions> optionsMonitor)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (optionsMonitor == null)
        {
            throw new ArgumentNullException(nameof(optionsMonitor));
        }

        _services = services;

        _optionsMonitor = optionsMonitor;

        _clients = new ConcurrentDictionary<string, Lazy<MemoryCacheClient>>();

        _clientFactory = (name) =>
        {
            return new Lazy<MemoryCacheClient>(() =>
            {
                return CreateClientHandler(name);
            });
        };
    }

    public MemoryCacheClient CreateClient(string name)
    {
        name ??= string.Empty;

        var client = _clients.GetOrAdd(name, _clientFactory);

        return client.Value;
    }

    internal MemoryCacheClient CreateClientHandler(string name)
    {
        var options = _optionsMonitor.Get(name);

        if (options == null)
        {
            throw new ArgumentException("No matching client found!");
        }

        var memoryCache = new MemoryCache(Options.Create(options));

        var factory = _services.GetRequiredService<IDistributedCacheClientFactory>();

        var distributedCacheClient = factory.CreateClient(name);

        return new MemoryCacheClient(memoryCache, distributedCacheClient, options.SubscribeKeyType, options.SubscribeKeyPrefix);
    }
}
