namespace MASA.Utils.Security.Cryptography;

public class AesUtils
{
    private static readonly byte[] Keys =
    {
        0x41,
        0x72,
        0x65,
        0x79,
        0x6F,
        0x75,
        0x6D,
        0x79,
        0x53,
        0x6E,
        0x6F,
        0x77,
        0x6D,
        0x61,
        0x6E,
        0x3F
    };

    /// <summary>
    /// Generate a key that complies with AES encryption rules
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateKey(int length)
    {
        var crypto = Aes.Create();
        crypto.KeySize = length;
        crypto.BlockSize = 128;
        crypto.GenerateKey();
        return Convert.ToBase64String(crypto.Key);
    }

    /// <summary>
    /// 对称加密算法AES RijndaelManaged加密(RijndaelManaged（AES）算法是块式加密算法)
    /// </summary>
    /// <param name="content">待加密字符串</param>
    /// <returns>加密结果字符串</returns>
    public static string Encrypt(string content)
        => Encrypt(content, GlobalConfigurationUtils.DefaultEncryKey);

    /// <summary>
    /// Symmetric encryption algorithm AES RijndaelManaged encryption (RijndaelManaged (AES) algorithm is a block encryption algorithm)
    /// </summary>
    /// <param name="content">String to be encrypted</param>
    /// <param name="key">Encryption key, must have half-width characters</param>
    /// <returns>encrypted result string</returns>
    public static string Encrypt(string content, string key)
    {
        key = GetSubString(key, 32, "");
        key = key.PadRight(32, ' ');
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.Substring(0, 32));
        aes.IV = Keys;
        using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
        byte[] buffers = Encoding.UTF8.GetBytes(content);
        byte[] encryptedData = cryptoTransform.TransformFinalBlock(buffers, 0, buffers.Length);
        return Convert.ToBase64String(encryptedData);
    }

    /// <summary>
    /// Symmetric encryption algorithm AES RijndaelManaged decrypts the string
    /// </summary>
    /// <param name="content">String to be decrypted</param>
    /// <returns>If the decryption succeeds, the decrypted string will be returned, and if it fails, the source string will be returned.</returns>
    public static string Decrypt(string content)
        => Decrypt(content, GlobalConfigurationUtils.DefaultEncryKey);

    /// <summary>
    /// Symmetric encryption algorithm AES RijndaelManaged decrypts the string
    /// </summary>
    /// <param name="content">String to be decrypted</param>
    /// <param name="key">Decryption key, same as encryption key</param>
    /// <returns>Decryption success returns the decrypted string, failure returns empty</returns>
    public static string Decrypt(string content, string key)
    {
        try
        {
            key = GetSubString(key, 32, "");
            key = key.PadRight(32, ' ');
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Keys;
            using ICryptoTransform rijndaelDecrypt = aes.CreateDecryptor();
            byte[] buffers = Convert.FromBase64String(content);
            byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(buffers, 0, buffers.Length);
            return Encoding.UTF8.GetString(decryptedData);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Get a part of a string by byte length (by byte, a Chinese character is 2 bytes)
    /// </summary>
    /// <param name="sourceString">source string</param>
    /// <param name="length">The length in bytes of the string taken</param>
    /// <param name="tailString">Additional string (when the string is not long enough, the string added at the end, usually "...")</param>
    /// <returns>某字符串的一部分</returns>
    private static string GetSubString(string sourceString, int length, string tailString)
    {
        return GetSubString(sourceString, 0, length, tailString);
    }

    /// <summary>
    /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
    /// </summary>
    /// <param name="sourceString">源字符串</param>
    /// <param name="startIndex">索引位置，以0开始</param>
    /// <param name="length">所取字符串字节长度</param>
    /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")</param>
    /// <returns>某字符串的一部分</returns>
    private static string GetSubString(string sourceString, int startIndex, int length, string tailString)
    {
        //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
        if (Regex.IsMatch(sourceString, "[\u0800-\u4e00]+") || Regex.IsMatch(sourceString, "[\xAC00-\xD7A3]+"))
        {
            //当截取的起始位置超出字段串长度时
            if (startIndex >= sourceString.Length)
            {
                return string.Empty;
            }

            return sourceString.Substring(startIndex,
                length + startIndex > sourceString.Length ? sourceString.Length - startIndex : length);
        }

        if (length <= 0)
        {
            return string.Empty;
        }

        byte[] bytesSource = Encoding.Default.GetBytes(sourceString);

        //当字符串长度大于起始位置
        if (bytesSource.Length > startIndex)
        {
            int endIndex = bytesSource.Length;

            //当要截取的长度在字符串的有效长度范围内
            if (bytesSource.Length > startIndex + length)
            {
                endIndex = length + startIndex;
            }
            else
            {
                //当不在有效范围内时,只取到字符串的结尾
                length = bytesSource.Length - startIndex;
                tailString = "";
            }

            var anResultFlag = new int[length];
            int nFlag = 0;
            //字节大于127为双字节字符
            for (int i = startIndex; i < endIndex; i++)
            {
                if (bytesSource[i] > 127)
                {
                    nFlag++;
                    if (nFlag == 3)
                    {
                        nFlag = 1;
                    }
                }
                else
                {
                    nFlag = 0;
                }

                anResultFlag[i] = nFlag;
            }

            //最后一个字节为双字节字符的一半
            if (bytesSource[endIndex - 1] > 127 && anResultFlag[length - 1] == 1)
            {
                length++;
            }

            byte[] bsResult = new byte[length];
            Array.Copy(bytesSource, startIndex, bsResult, 0, length);
            var myResult = Encoding.Default.GetString(bsResult);
            myResult += tailString;
            return myResult;
        }

        return string.Empty;
    }

    /// <summary>
    /// 加密文件流
    /// </summary>
    /// <param name="fileStream">需要加密的文件流</param>
    /// <param name="key">加密密钥</param>
    /// <returns>加密流</returns>
    public static CryptoStream Encrypt(FileStream fileStream, string key)
    {
        key = GetSubString(key, 32, "");
        key = key.PadRight(32, ' ');
        using var rijndaelProvider = new RijndaelManaged()
        {
            Key = Encoding.UTF8.GetBytes(key),
            IV = Keys
        };
        using var cryptoTransform = rijndaelProvider.CreateEncryptor();
        return new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write);
    }

    /// <summary>
    /// 解密文件流
    /// </summary>
    /// <param name="fileStream">需要解密的文件流</param>
    /// <param name="key">解密密钥</param>
    /// <returns>加密流</returns>
    public static CryptoStream Decrypt(FileStream fileStream, string key)
    {
        key = GetSubString(key, 32, "");
        key = key.PadRight(32, ' ');
        using var rijndaelProvider = new RijndaelManaged()
        {
            Key = Encoding.UTF8.GetBytes(key),
            IV = Keys
        };
        using var cryptoTransform = rijndaelProvider.CreateDecryptor();
        return new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Read);
    }

    /// <summary>
    /// 对指定文件AES加密
    /// </summary>
    /// <param name="fileStream">源文件流</param>
    /// <param name="outputPath">输出文件路径</param>
    public static void EncryptFile(FileStream fileStream, string outputPath)
    {
        using var fileStreamOut = new FileStream(outputPath, FileMode.Create);
        using var cryptoStream = Encrypt(fileStream, GlobalConfigurationUtils.DefaultEncryKey);
        byte[] buffers = new byte[1024];
        while (true)
        {
            var count = cryptoStream.Read(buffers, 0, buffers.Length);
            fileStreamOut.Write(buffers, 0, count);
            if (count < buffers.Length)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 对指定的文件AES解密
    /// </summary>
    /// <param name="fileStream">源文件流</param>
    /// <param name="outputPath">输出文件路径</param>
    public static void DecryptFile(FileStream fileStream, string outputPath)
    {
        using FileStream fileStreamOut = new(outputPath, FileMode.Create);
        using CryptoStream cryptoStream = Decrypt(fileStream, GlobalConfigurationUtils.DefaultEncryKey);
        byte[] buffers = new byte[1024];
        while (true)
        {
            var count = cryptoStream.Read(buffers, 0, buffers.Length);
            fileStreamOut.Write(buffers, 0, count);
            if (count < buffers.Length)
            {
                break;
            }
        }
    }
}
