namespace Masa.Utils.Data.Mapping;

public interface IMappingConfigProvider
{
    TypeAdapterConfig GetConfig(Type sourceType, Type destinationType, MapOptions? options = null);
}
