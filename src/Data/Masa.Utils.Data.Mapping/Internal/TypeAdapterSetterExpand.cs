namespace Masa.Utils.Data.Mapping.Internal;

internal class TypeAdapterSetterExpand
{
    public static TypeAdapterSetter<TSource, TDestination> NewConfigByConstructor<TSource, TDestination>(TypeAdapterConfig adapterConfig,
        object constructorInfo)
    {
        return adapterConfig
            .NewConfig<TSource, TDestination>()
            .MapToConstructor((constructorInfo as ConstructorInfo)!);
    }
}
