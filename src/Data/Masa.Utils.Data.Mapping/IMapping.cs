namespace Masa.Utils.Data.Mapping;

public interface IMapping
{
    TDestination Map<TSource, TDestination>(TSource source, MapOptions? options = null);

    TDestination Map<TDestination>(object source, MapOptions? options = null);
}
