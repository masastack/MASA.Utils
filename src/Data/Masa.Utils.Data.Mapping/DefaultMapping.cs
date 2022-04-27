namespace Masa.Utils.Data.Mapping;

public class DefaultMapping : IMapping
{
    private readonly IMappingConfigProvider _provider;

    public DefaultMapping(IMappingConfigProvider provider)
        => _provider = provider;

    public TDestination Map<TSource, TDestination>(TSource source, MapOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.Adapt<TSource, TDestination>(_provider.GetConfig(source.GetType(), typeof(TDestination), options));
    }

    public TDestination Map<TDestination>(object source, MapOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.Adapt<TDestination>(_provider.GetConfig(source.GetType(), typeof(TDestination), options));
    }
}
