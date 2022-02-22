namespace MASA.Utils.Security.Cryptography;

public class GlobalConfigurationUtils
{
    private static string _defaultEncryKey = "masastack.com";

    public static string DefaultEncryKey
    {
        get => _defaultEncryKey;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{nameof(DefaultEncryKey)} cannot be empty", nameof(DefaultEncryKey));

            _defaultEncryKey = value;
        }
    }
}
