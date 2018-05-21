using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ConversionLib.Cryptography
{
    public static class SymmetricEncryption
    {
        public const int KeySize = 256 / 8;
        public const int SaltSize = 128 / 8;

        static SymmetricAlgorithm CreateAlgorithm() => new RijndaelManaged();

        static SymmetricAlgorithm CreateAlgorithm(byte[] key)
        {
            var algorithm = CreateAlgorithm();
            algorithm.Key = key;
            algorithm.BlockSize = 8 * SaltSize;
            algorithm.IV = new byte[SaltSize];
            return algorithm;
        }

        public static byte[] GenerateKey() => CryptoHelper.GenerateBytes(KeySize);
        public static string GenerateKeyBase64() => CryptoHelper.GenerateBase64(KeySize);

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

        public static void Encrypt(Stream input, Stream output, byte[] key) => Transform(input, output, key, true);
        public static void Decrypt(Stream input, Stream output, byte[] key) => Transform(input, output, key, false);

        static void Transform(Stream input, Stream output, byte[] key, bool encrypt)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (key == null) throw new ArgumentNullException(nameof(key));

            using (var algorithm = CreateAlgorithm(key))
            using (var transform = encrypt ? algorithm.CreateEncryptor() : algorithm.CreateDecryptor())
            using (var crypto = new CryptoStream(output, transform, CryptoStreamMode.Write))
            {
                input.CopyTo(crypto);
            }
        }

        public static void Encrypt(Stream input, Stream output, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Encrypt(input, output, Convert.FromBase64String(key));
        }

        public static void Decrypt(Stream input, Stream output, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Decrypt(input, output, Convert.FromBase64String(key));
        }

        public static string EncryptByPassword(string data, string password)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var key = HashFunctions.GenerateHashByRfc2898(password, KeySize);
            return Encrypt(data, key);
        }

        public static string DecryptByPassword(string data, string password)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var key = HashFunctions.GenerateHashByRfc2898(password, KeySize);
            return Decrypt(data, key);
        }
    }
}
