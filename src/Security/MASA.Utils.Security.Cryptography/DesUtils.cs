namespace MASA.Utils.Security.Cryptography
{
    /// <summary>
    /// DES symmetric encryption and decryption
    /// </summary>
    public class DesUtils : EncryptBase
    {
        /// <summary>
        /// Default encryption key
        /// </summary>
        private static readonly string DefaultEncryptKey = MD5Utils.EncryptRepeat(GlobalConfigurationUtils.DefaultEncryKey, 2);

        /// <summary>
        /// 使用默认加密
        /// </summary>
        /// <param name="content">被加密的字符串</param>
        /// <param name="desEncryType">Des encryption method, default: improved (easy to transmit)</param>
        /// <param name="isToLower">Whether to convert the encrypted string to lowercase</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        /// <returns>encrypted result</returns>
        public static string Encrypt(
            string content,
            DESEncryType desEncryType = DESEncryType.Improved,
            bool isToLower = true,
            Encoding? encoding = null)
            => Encrypt(content, DefaultEncryptKey, desEncryType, isToLower, encoding);

        /// <summary>
        /// Des encrypted string
        /// </summary>
        /// <param name="content">String to be encrypted</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="desEncryType">Des encryption method, default: improved (easy to transmit)</param>
        /// <param name="isToLower">Whether to convert the encrypted string to lowercase</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        /// <returns>encrypted result</returns>
        public static string Encrypt(
            string content,
            string key,
            DESEncryType desEncryType = DESEncryType.Improved,
            bool isToLower = true,
            Encoding? encoding = null)
            => Encrypt(content, key, key, desEncryType, isToLower, encoding);

        /// <summary>
        /// Des encrypted string
        /// </summary>
        /// <param name="content">String to be encrypted</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="iv">8-bit length key</param>
        /// <param name="desEncryType">Des encryption method, default: improved (easy to transmit)</param>
        /// <param name="isToLower">Whether to convert the encrypted string to lowercase</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        /// <returns>encrypted result</returns>
        public static string Encrypt(
            string content,
            string key,
            string iv,
            DESEncryType desEncryType = DESEncryType.Improved,
            bool isToLower = true,
            Encoding? encoding = null)
        {
            if (key.Length < 8)
            {
                throw new Exception("The key length is invalid. The key cannot be less than 8！");
            }

            if (!string.IsNullOrEmpty(iv) && iv.Length < 8)
            {
                throw new Exception($"The {nameof(iv)} length is invalid. The {nameof(iv)} cannot be less than 8！");
            }

            var currentEncoding = GetSafeEncoding(encoding);
            var des = DES.Create();
            des.Key = currentEncoding.GetBytes(GetSpecifiedNumberString(key, 8));
            des.IV = des.Key;
            if (!string.IsNullOrEmpty(iv))
            {
                des.IV = currentEncoding.GetBytes(GetSpecifiedNumberString(iv, 8));
            }

            using MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = currentEncoding.GetBytes(content);
            using CryptoStream cs = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(buffer, 0, buffer.Length);
            cs.FlushFinalBlock();
            if (desEncryType == DESEncryType.Normal)
                return Convert.ToBase64String(memoryStream.ToArray());

            StringBuilder stringBuilder = new();
            foreach (byte b in memoryStream.ToArray())
            {
                stringBuilder.AppendFormat(isToLower ? $"{b:x2}" : $"{b:X2}");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// DES decryption with default key
        /// </summary>
        /// <param name="content">String to be decrypted</param>
        /// <param name="desEncryType">Des encryption method, default: improved (easy to transmit)</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        /// <returns>decrypted result</returns>
        public static string Decrypt(string content,
            DESEncryType desEncryType = DESEncryType.Improved,
            Encoding? encoding = null)
            => Decrypt(content, DefaultEncryptKey, desEncryType, encoding);

        /// <summary>
        /// DES decryption
        /// </summary>
        /// <param name="content">String to be decrypted</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="desEncryType">Des encryption method, default: improved (easy to transmit)</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        /// <returns>decrypted result</returns>
        public static string Decrypt(
            string content,
            string key,
            DESEncryType desEncryType = DESEncryType.Improved,
            Encoding? encoding = null)
            => Decrypt(content, key, key, desEncryType, encoding);

        /// <summary>
        /// DES decryption
        /// </summary>
        /// <param name="content">String to be decrypted</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="iv">8-bit length key</param>
        /// <param name="desEncryType">Des encryption method, default: improved (easy to transmit)</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        /// <returns>decrypted result</returns>
        public static string Decrypt(
            string content,
            string key,
            string iv,
            DESEncryType desEncryType = DESEncryType.Improved,
            Encoding? encoding = null)
        {
            if (key.Length < 8)
            {
                throw new Exception($"The {nameof(key)} length is invalid. The {nameof(key)} cannot be less than 8！");
            }

            if (!string.IsNullOrEmpty(iv) && iv.Length < 8)
            {
                throw new Exception($"The {nameof(iv)} length is invalid. The {nameof(iv)} cannot be less than 8！");
            }

            using var memoryStream = new MemoryStream();
            using var des = DES.Create();
            var currentEncoding = GetSafeEncoding(encoding);
            byte[] byteKey = currentEncoding.GetBytes(GetSpecifiedNumberString(key, 8));
            byte[] byteIv = byteKey;
            if (!string.IsNullOrEmpty(iv))
            {
                byteIv = currentEncoding.GetBytes(GetSpecifiedNumberString(iv, 8));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffers = desEncryType == DESEncryType.Improved ? new byte[content.Length / 2] : Convert.FromBase64String(content);
                if (desEncryType == DESEncryType.Improved)
                {
                    for (int x = 0; x < content.Length / 2; x++)
                    {
                        int i = Convert.ToInt32(content.Substring(x * 2, 2), 16);
                        buffers[x] = (byte) i;
                    }
                }

                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byteKey, byteIv), CryptoStreamMode.Write))
                {
                    cs.Write(buffers, 0, buffers.Length);
                    cs.FlushFinalBlock();
                }

                return currentEncoding.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// DES encrypts the file stream and outputs the encrypted file
        /// </summary>
        /// <param name="fileStream">file input stream</param>
        /// <param name="outFilePath">file output path</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="iv">8-bit length key</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        public static void EncryptFile(
            FileStream fileStream,
            string outFilePath,
            string key,
            string iv,
            Encoding? encoding = null)
        {
            if (!string.IsNullOrEmpty(iv) && iv.Length < 8)
            {
                throw new Exception($"The {nameof(iv)} length is invalid. The {nameof(iv)} cannot be less than 8！");
            }

            var currentEncoding = GetSafeEncoding(encoding);
            var ivBuffer = string.IsNullOrEmpty(iv)
                ? currentEncoding.GetBytes(GetSpecifiedNumberString(key, 8))
                : currentEncoding.GetBytes(GetSpecifiedNumberString(iv, 8));
            EncryptFile(fileStream, outFilePath, key, ivBuffer, encoding);
        }

        /// <summary>
        /// DES encrypts the file stream and outputs the encrypted file
        /// </summary>
        /// <param name="fileStream">file input stream</param>
        /// <param name="outFilePath">file output path</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="iv">8-bit length key</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        public static void EncryptFile(
            FileStream fileStream,
            string outFilePath,
            string key,
            byte[] iv,
            Encoding? encoding = null)
        {
            if (key.Length < 8)
            {
                throw new Exception($"The {nameof(key)} length is invalid. The {nameof(key)} cannot be less than 8！");
            }

            var currentEncoding = GetSafeEncoding(encoding);
            using var fileStreamOut = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStreamOut.SetLength(0);
            byte[] buffers = new byte[100];
            long readLength = 0;
            using var des = DES.Create();
            des.Key = currentEncoding.GetBytes(GetSpecifiedNumberString(key, 8));
            des.IV = iv;

            using var cryptoStream = new CryptoStream(fileStreamOut, des.CreateEncryptor(),
                CryptoStreamMode.Write);
            while (readLength < fileStream.Length)
            {
                var length = fileStream.Read(buffers, 0, 100);
                cryptoStream.Write(buffers, 0, length);
                readLength += length;
            }
        }

        /// <summary>
        /// DES encrypts the file stream and outputs the encrypted file
        /// </summary>
        /// <param name="fileStream">file input stream</param>
        /// <param name="outFilePath">file output path</param>
        /// <param name="key">8-bit length key</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        public static void EncryptFile(
            FileStream fileStream,
            string outFilePath,
            string key,
            Encoding? encoding = null)
        {
            byte[] iv =
            {
                0x12,
                0x34,
                0x56,
                0x78,
                0x90,
                0xAB,
                0xCD,
                0xEF
            };
            EncryptFile(fileStream, outFilePath, key, iv, encoding);
        }

        /// <summary>
        /// DES decrypts the file stream and outputs the source file
        /// </summary>
        /// <param name="fileStream">input file stream to be decrypted</param>
        /// <param name="outFilePath">file output path</param>
        /// <param name="key">decryption key</param>
        /// <param name="iv">8-bit length key</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        public static void DecryptFile(
            FileStream fileStream,
            string outFilePath,
            string key,
            string iv,
            Encoding? encoding = null)
        {
            if (!string.IsNullOrEmpty(iv) && iv.Length < 8)
            {
                throw new Exception($"The {nameof(iv)} length is invalid. The {nameof(iv)} cannot be less than 8！");
            }

            var currentEncoding = GetSafeEncoding(encoding);

            var ivBuffer = string.IsNullOrEmpty(iv)
                ? currentEncoding.GetBytes(GetSpecifiedNumberString(key, 8))
                : currentEncoding.GetBytes(GetSpecifiedNumberString(iv, 8));

            DecryptFile(fileStream, outFilePath, key, ivBuffer, currentEncoding);
        }

        /// <summary>
        /// DES decrypts the file stream and outputs the source file
        /// </summary>
        /// <param name="fileStream">input file stream to be decrypted</param>
        /// <param name="outFilePath">file output path</param>
        /// <param name="key">decryption key</param>
        /// <param name="iv"></param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        public static void DecryptFile(
            FileStream fileStream,
            string outFilePath,
            string key,
            byte[] iv,
            Encoding? encoding = null)
        {
            if (!string.IsNullOrEmpty(key) && key.Length < 8)
            {
                throw new Exception($"The {nameof(key)} length is invalid. The {nameof(key)} cannot be less than 8！");
            }

            var currentEncoding = GetSafeEncoding(encoding);
            var byKeys = currentEncoding.GetBytes(GetSpecifiedNumberString(key, 8));
            using var fileStreamOut = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStreamOut.SetLength(0);
            byte[] buffers = new byte[100];
            long readLength = 0;
            using var des = DES.Create();

            using var cryptoStream = new CryptoStream(fileStreamOut, des.CreateDecryptor(byKeys, iv),
                CryptoStreamMode.Write);
            while (readLength < fileStream.Length)
            {
                var length = fileStream.Read(buffers, 0, 100);
                cryptoStream.Write(buffers, 0, length);
                readLength += length;
            }
        }

        /// <summary>
        /// DES decrypts the file stream and outputs the source file
        /// </summary>
        /// <param name="fileStream">input file stream to be decrypted</param>
        /// <param name="outFilePath">file output path</param>
        /// <param name="key">decryption key</param>
        /// <param name="encoding">Encoding format, default UTF-8</param>
        public static void DecryptFile(
            FileStream fileStream,
            string outFilePath,
            string key,
            Encoding? encoding = null)
        {
            byte[] iv =
            {
                0x12,
                0x34,
                0x56,
                0x78,
                0x90,
                0xAB,
                0xCD,
                0xEF
            };
            DecryptFile(fileStream, outFilePath, key, iv, encoding);
        }
    }
}
