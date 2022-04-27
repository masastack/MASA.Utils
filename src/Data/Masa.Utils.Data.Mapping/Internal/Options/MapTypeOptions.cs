namespace Masa.Utils.Data.Mapping.Internal.Options;

internal class MapTypeOptions
{
    public Type SourceType { get; } = default!;

    public Type DestinationType { get; } = default!;

    public ConstructorInfo Constructor { get; set; } = default!;

    public MapTypeOptions(Type sourceType, Type destinationType)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
    }
}
