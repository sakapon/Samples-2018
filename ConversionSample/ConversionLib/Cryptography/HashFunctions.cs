using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ConversionLib.Cryptography
{
    public static class HashFunctions
    {
        internal static HashFunctionBase Algorithm { get; set; } = new Rfc2898Hash();

        public static int HashSize => Algorithm.HashSize;
        public static int SaltSize => Algorithm.SaltSize;

        public static byte[] GenerateSalt() => CryptoHelper.GenerateBytes(SaltSize);
        public static string GenerateSaltBase64() => CryptoHelper.GenerateBase64(SaltSize);

        public static byte[] GenerateHash(byte[] data) => Algorithm.GenerateHash(data);
        public static string GenerateHash(string data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(GenerateHash(data.ToBytes()));
        }

        public static byte[] GenerateHash(byte[] data, byte[] salt) => Algorithm.GenerateHash(data, salt);
        public static string GenerateHash(string data, string salt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            return Convert.ToBase64String(GenerateHash(data.ToBytes(), Convert.FromBase64String(salt)));
        }

        public static bool VerifyByHash(string data, string hash) => GenerateHash(data) == hash;
        public static bool VerifyByHash(string data, string salt, string hash) => GenerateHash(data, salt) == hash;

        public static string GenerateHashWithSalt(string data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var salt = GenerateSalt();
            var hash = GenerateHash(data.ToBytes(), salt);

            // Crypto.cs implements the hash format as { 0x00, salt, subkey }.
            // https://github.com/aspnetwebstack/aspnetwebstack/blob/master/src/System.Web.Helpers/Crypto.cs
            CryptoHelper.Concat(salt, hash, out var hashWithSalt);
            return Convert.ToBase64String(hashWithSalt);
        }

        public static bool VerifyByHashWithSalt(string data, string hashWithSalt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (hashWithSalt == null) throw new ArgumentNullException(nameof(hashWithSalt));

            var hashWithSaltBytes = Convert.FromBase64String(hashWithSalt);
            if (hashWithSaltBytes.Length != SaltSize + HashSize) throw new ArgumentException("The size of the byte array is invalid.", nameof(hashWithSalt));

            CryptoHelper.Separate(out var salt, out var hash, hashWithSaltBytes, SaltSize);
            return GenerateHash(data.ToBytes(), salt).ByteArrayEqual(hash);
        }

        public static byte[] GenerateHashByRfc2898(byte[] data, int hashSize)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            using (var algorithm = new Rfc2898DeriveBytes(data, new byte[16], 10000))
            {
                return algorithm.GetBytes(hashSize);
            }
        }

        public static string GenerateHashByRfc2898(string data, int hashSize)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(GenerateHashByRfc2898(data.ToBytes(), hashSize));
        }
    }
}
