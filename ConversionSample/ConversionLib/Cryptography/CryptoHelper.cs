using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConversionLib.Cryptography
{
    public static class CryptoHelper
    {
        public static byte[] GenerateBytes(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), length, "The value must be non-negative.");

            var data = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }
            return data;
        }

        public static string GenerateBase64(int length) => Convert.ToBase64String(GenerateBytes(length));

        // The Encoding.UTF8.GetBytes method does not prepend a preamble to the encoded byte sequence.
        internal static byte[] ToBytes(this string data) => Encoding.UTF8.GetBytes(data);
        internal static string ToText(this byte[] data) => Encoding.UTF8.GetString(data);
    }
}
