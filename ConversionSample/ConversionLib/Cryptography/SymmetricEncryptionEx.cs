using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using static ConversionLib.Cryptography.SymmetricEncryption;

namespace ConversionLib.Cryptography
{
    public static class SymmetricEncryptionEx
    {
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

        public static void EncryptWithCompression(Stream input, Stream output, string key)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (key == null) throw new ArgumentNullException(nameof(key));

            using (var algorithm = CreateAlgorithm(Convert.FromBase64String(key)))
            using (var transform = algorithm.CreateEncryptor())
            using (var crypto = new CryptoStream(output, transform, CryptoStreamMode.Write))
            using (var gzip = new GZipStream(crypto, CompressionMode.Compress, true))
            {
                input.CopyTo(gzip);
            }
        }

        public static void DecryptWithDecompression(Stream input, Stream output, string key)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (key == null) throw new ArgumentNullException(nameof(key));

            using (var algorithm = CreateAlgorithm(Convert.FromBase64String(key)))
            using (var transform = algorithm.CreateDecryptor())
            using (var crypto = new CryptoStream(input, transform, CryptoStreamMode.Read))
            using (var gzip = new GZipStream(crypto, CompressionMode.Decompress, true))
            {
                gzip.CopyTo(output);
            }
        }
    }
}
