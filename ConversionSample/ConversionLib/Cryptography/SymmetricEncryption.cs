using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConversionLib.Cryptography
{
    public static class SymmetricEncryption
    {
        const int KeySize = 256 / 8;
        const int SaltSize = 128 / 8;

        static SymmetricAlgorithm CreateAlgorithm() => new RijndaelManaged();

        static SymmetricAlgorithm CreateAlgorithm(byte[] key)
        {
            var algorithm = CreateAlgorithm();
            algorithm.Key = key;
            algorithm.IV = new byte[SaltSize];
            return algorithm;
        }

        // The Encoding.UTF8.GetBytes method does not prepend a preamble to the encoded byte sequence.
        static byte[] ToBytes(this string data) => Encoding.UTF8.GetBytes(data);
        static string ToText(this byte[] data) => Encoding.UTF8.GetString(data);

        public static byte[] GenerateKey() => RandomHelper.GenerateBytes(KeySize);
        public static string GenerateKeyBase64() => RandomHelper.GenerateBase64(KeySize);

        public static byte[] Encrypt(byte[] data, byte[] key) => Transform(data, key, true);
        public static byte[] Decrypt(byte[] data, byte[] key) => Transform(data, key, false);

        static byte[] Transform(byte[] data, byte[] key, bool encrypt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));

            using (var algorithm = CreateAlgorithm(key))
            using (var transform = encrypt ? algorithm.CreateEncryptor() : algorithm.CreateDecryptor())
            {
                return transform.TransformFinalBlock(data, 0, data.Length);
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
