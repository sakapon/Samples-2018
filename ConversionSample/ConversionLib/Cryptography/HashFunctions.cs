using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blaze.Randomization;

namespace ConversionLib.Cryptography
{
    public static class HashFunctions
    {
        internal static HashFunctionBase Algorithm { get; set; } = new Rfc2898Hash();

        public static int HashLength => Algorithm.HashLength;
        public static int SaltLength => Algorithm.SaltLength;

        // The Encoding.UTF8.GetBytes method does not prepend a preamble to the encoded byte sequence.
        static readonly Encoding TextEncoding = Encoding.UTF8;

        public static byte[] GenerateSalt() => RandomData.GenerateBytes(SaltLength);
        public static string GenerateSaltString() => Convert.ToBase64String(GenerateSalt());

        public static string GenerateHash(string data, string salt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            return Convert.ToBase64String(Algorithm.GenerateHash(TextEncoding.GetBytes(data), Convert.FromBase64String(salt)));
        }

        public static bool VerifyData(string data, string salt, string hash) => GenerateHash(data, salt) == hash;
    }
}
