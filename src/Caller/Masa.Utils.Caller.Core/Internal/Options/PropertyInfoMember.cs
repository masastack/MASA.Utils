namespace Masa.Utils.Caller.Core.Internal.Options;

internal class PropertyInfoMember
{
    public PropertyInfo Property { get; }

    public string Name { get; }

    public bool NeedSerialize { get; }

    public PropertyInfoMember(PropertyInfo property, string name, bool needSerialize)
    {
        Property = property;
        Name = name;
        NeedSerialize = needSerialize;
    }

    public bool TryGetValue<TRequest>(TRequest data, out string value) where TRequest : class
    {
        value = string.Empty;
        var propertyValue = Property.GetValue(data);
        if (propertyValue == null || (!NeedSerialize && propertyValue.ToString() == null))
            return false;

        value = !NeedSerialize ? propertyValue.ToString()! : JsonSerializer.Serialize(propertyValue);
        return true;
    }
}
