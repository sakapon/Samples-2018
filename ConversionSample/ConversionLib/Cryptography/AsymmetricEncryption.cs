using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ConversionLib.Cryptography
{
    public static class AsymmetricEncryption
    {
        public const int KeyLengthInBits = 2048;

        public static AsymmetricKeys CreateKeys()
        {
            using (var algorithm = new RSACryptoServiceProvider(KeyLengthInBits))
            {
                return new AsymmetricKeys
                {
                    PublicKey = algorithm.ToXmlString(false),
                    KeysPair = algorithm.ToXmlString(true),
                };
            }
        }

        public static byte[] Encrypt(byte[] data, string publicKey)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));

            using (var algorithm = new RSACryptoServiceProvider())
            {
                algorithm.FromXmlString(publicKey);

                return algorithm.Encrypt(data, true);
            }
        }

        public static byte[] Decrypt(byte[] data, string keysPair)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (keysPair == null) throw new ArgumentNullException(nameof(keysPair));

            using (var algorithm = new RSACryptoServiceProvider())
            {
                algorithm.FromXmlString(keysPair);

                return algorithm.Decrypt(data, true);
            }
        }
    }

    public class AsymmetricKeys
    {
        public string PublicKey { get; internal set; }
        public string KeysPair { get; internal set; }
    }
}
