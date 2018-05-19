using System;
using System.Collections.Generic;
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
            var encryptedData = SymmetricEncryption.Encrypt(data, commonKey);
            var encryptedKey = AsymmetricEncryption.Encrypt(commonKey, publicKey);

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
    }
}
