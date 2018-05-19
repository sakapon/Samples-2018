using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConversionLib.Cryptography
{
    public static class HashFunctions
    {
        internal static HashFunctionBase Algorithm { get; set; } = new Rfc2898Hash();

        public static int HashLength => Algorithm.HashLength;
        public static int SaltLength => Algorithm.SaltLength;

        // The Encoding.UTF8.GetBytes method does not prepend a preamble to the encoded byte sequence.
        static readonly Encoding TextEncoding = Encoding.UTF8;

        static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static byte[] GenerateBytes(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), length, "The value must be non-negative.");

            var data = new byte[length];
            Rng.GetBytes(data);
            return data;
        }

        public static byte[] GenerateSalt() => GenerateBytes(SaltLength);
        public static string GenerateSaltBase64() => Convert.ToBase64String(GenerateSalt());

        public static byte[] GenerateHash(byte[] data) => Algorithm.GenerateHash(data);
        public static string GenerateHash(string data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(GenerateHash(TextEncoding.GetBytes(data)));
        }

        public static byte[] GenerateHash(byte[] data, byte[] salt) => Algorithm.GenerateHash(data, salt);
        public static string GenerateHash(string data, string salt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            return Convert.ToBase64String(GenerateHash(TextEncoding.GetBytes(data), Convert.FromBase64String(salt)));
        }

        public static bool VerifyByHash(string data, string salt, string hash) => GenerateHash(data, salt) == hash;

        public static string GenerateHashWithSalt(string data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var salt = GenerateSalt();
            var hash = GenerateHash(TextEncoding.GetBytes(data), salt);
            ToHashWithSalt(salt, hash, out var result);
            return Convert.ToBase64String(result);
        }

        public static bool VerifyByHashWithSalt(string data, string hashWithSalt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (hashWithSalt == null) throw new ArgumentNullException(nameof(hashWithSalt));

            FromHashWithSalt(out var salt, out var hash, Convert.FromBase64String(hashWithSalt));
            return GenerateHash(TextEncoding.GetBytes(data), salt).ByteArrayEqual(hash);
        }

        // Crypto.cs implements the hash format as { 0x00, salt, subkey }.
        // https://github.com/aspnetwebstack/aspnetwebstack/blob/master/src/System.Web.Helpers/Crypto.cs
        static void ToHashWithSalt(byte[] salt, byte[] hash, out byte[] hashWithSalt)
        {
            hashWithSalt = new byte[SaltLength + HashLength];
            Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, hashWithSalt, salt.Length, hash.Length);
        }

        static void FromHashWithSalt(out byte[] salt, out byte[] hash, byte[] hashWithSalt)
        {
            salt = new byte[SaltLength];
            hash = new byte[HashLength];
            Buffer.BlockCopy(hashWithSalt, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(hashWithSalt, salt.Length, hash, 0, hash.Length);
        }

        static bool ByteArrayEqual(this byte[] first, byte[] second)
        {
            if (Equals(first, second)) return true;
            if (first == null || second == null) return false;
            if (first.Length != second.Length) return false;

            for (var i = 0; i < first.Length; i++)
                if (first[i] != second[i]) return false;
            return true;
        }
    }
}
