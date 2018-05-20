using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConversionLib.Cryptography
{
    public static class HybridEncryption
    {
        public static AsymmetricKeys GenerateKeys() => AsymmetricEncryption.GenerateKeys();

        public static byte[] Encrypt(byte[] data, string publicKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));

            var commonKey = SymmetricEncryption.GenerateKey();
            var encryptedKey = AsymmetricEncryption.Encrypt(commonKey, publicKey);
            var encryptedData = SymmetricEncryption.Encrypt(data, commonKey);

            CryptoHelper.Concat(encryptedKey, encryptedData, out var result);
            return result;
        }

        public static byte[] Decrypt(byte[] data, string keysPair)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (keysPair == null) throw new ArgumentNullException(nameof(keysPair));

            CryptoHelper.Separate(out var encryptedKey, out var encryptedData, data, AsymmetricEncryption.KeySize);

            var commonKey = AsymmetricEncryption.Decrypt(encryptedKey, keysPair);
            return SymmetricEncryption.Decrypt(encryptedData, commonKey);
        }

        public static string Encrypt(string data, string publicKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(Encrypt(data.ToBytes(), publicKey));
        }

        public static string Decrypt(string data, string keysPair)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return Decrypt(Convert.FromBase64String(data), keysPair).ToText();
        }

        public static void Encrypt(Stream input, Stream output, string publicKey)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));

            var commonKey = SymmetricEncryption.GenerateKey();
            var encryptedKey = AsymmetricEncryption.Encrypt(commonKey, publicKey);

            output.Write(encryptedKey, 0, encryptedKey.Length);
            SymmetricEncryption.Encrypt(input, output, commonKey);
        }

        public static void Decrypt(Stream input, Stream output, string keysPair)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (keysPair == null) throw new ArgumentNullException(nameof(keysPair));

            var encryptedKey = new byte[AsymmetricEncryption.KeySize];
            input.Read(encryptedKey, 0, encryptedKey.Length);

            var commonKey = AsymmetricEncryption.Decrypt(encryptedKey, keysPair);
            SymmetricEncryption.Decrypt(input, output, commonKey);
        }
    }
}
