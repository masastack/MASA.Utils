namespace MASA.Utils.Security.Cryptography;

public class EncryptBase
{
    protected static string GetSpecifiedNumberString(string key, int number) => key.Length > number ? key.Substring(0, number) : key;

    protected static Encoding GetSafeEncoding(Encoding? encoding = null)
        => GetSafeEncoding(() => Encoding.UTF8, encoding);

    protected static Encoding GetSafeEncoding(Func<Encoding> func, Encoding? encoding = null)
        => encoding ?? func.Invoke();
}
