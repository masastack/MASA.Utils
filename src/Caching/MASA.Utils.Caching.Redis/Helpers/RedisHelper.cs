namespace MASA.Utils.Caching.Redis.Helpers;

public static class RedisHelper
{
    public static T ConvertToValue<T>(RedisValue redisValue)
    {
        if (typeof(T).IsNumericType())
        {
            return (dynamic)(long)redisValue;
        }

        var byteValue = (byte[])redisValue;

        if (byteValue == null || byteValue.Length == 0)
            return default;

        var value = Decompress(byteValue);

        if (typeof(T).Equals(typeof(string)))
        {
            var valueString = Encoding.UTF8.GetString(value);
            return (dynamic)valueString;
        }

        var options = new JsonSerializerOptions();
        options.EnableDynamicTypes();

        return JsonSerializer.Deserialize<T>(value, options);
    }

    public static dynamic ConvertFromValue<T>(T value)
    {
        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return (dynamic)value;
            case TypeCode.String:
                return Compress(Encoding.UTF8.GetBytes(value.ToString()));
            default:
                var options = new JsonSerializerOptions();
                options.EnableDynamicTypes();

                var jsonString = JsonSerializer.Serialize(value, options);
                return Compress(Encoding.UTF8.GetBytes(jsonString));
        }
    }

    public static byte[] Compress(byte[] data)
    {
        using (MemoryStream msGZip = new MemoryStream())
        using (GZipStream stream = new GZipStream(msGZip, CompressionMode.Compress, true))
        {
            stream.Write(data, 0, data.Length);
            stream.Close();
            return msGZip.ToArray();
        }
    }

    public static byte[] Decompress(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        using (GZipStream stream = new GZipStream(ms, CompressionMode.Decompress))
        using (MemoryStream outBuffer = new MemoryStream())
        {
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            return outBuffer.ToArray();
        }
    }

    public static bool IsNumericType(this Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}
