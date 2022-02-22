namespace System;

public class EnumUtil
{
    public static T? GetSubitemAttribute<T>(object enumSubitem)
        where T : Attribute, new()
    {
        if (enumSubitem == null)
            return null;

        string value = enumSubitem.ToString() ?? "";

        var fieldInfo = enumSubitem.GetType().GetField(value);

        if (fieldInfo != null)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(T), false);

            if (attributes == null || attributes.Length == 0)
            {
                return new T();
            }
            else
            {
                return attributes[0] as T;
            }
        }
        else
        {
            return new T();
        }
    }
}
