using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ConversionLib.Cryptography
{
    public static class SymmetricEncryption_Old
    {
        static SymmetricAlgorithm CreateAlgorithm(byte[] key)
        {
            var algorithm = new RijndaelManaged();
            algorithm.Key = key;
            algorithm.IV = new byte[algorithm.BlockSize / 8];
            return algorithm;
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            using (var algorithm = CreateAlgorithm(key))
            using (var transform = algorithm.CreateEncryptor())
            using (var crypto = new CryptoStream(output, transform, CryptoStreamMode.Write))
            {
                input.CopyTo(crypto);
                crypto.FlushFinalBlock();
                return output.ToArray();
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            using (var algorithm = CreateAlgorithm(key))
            using (var transform = algorithm.CreateDecryptor())
            using (var crypto = new CryptoStream(input, transform, CryptoStreamMode.Read))
            {
                crypto.CopyTo(output);
                return output.ToArray();
            }
        }

        public static string Encrypt(string data, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Convert.ToBase64String(Encrypt(data.ToBytes(), Convert.FromBase64String(key)));
        }

        public static string Decrypt(string data, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Decrypt(Convert.FromBase64String(data), Convert.FromBase64String(key)).ToText();
        }
    }
}
