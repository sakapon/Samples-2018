using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConversionLib.Cryptography
{
    public static class CryptoHelper
    {
        public static byte[] GenerateBytes(int size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size), size, "The value must be non-negative.");

            var data = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }
            return data;
        }

        public static string GenerateBase64(int size) => Convert.ToBase64String(GenerateBytes(size));

        // The Encoding.UTF8.GetBytes method does not prepend a preamble to the encoded byte sequence.
        internal static byte[] ToBytes(this string data) => Encoding.UTF8.GetBytes(data);
        internal static string ToText(this byte[] data) => Encoding.UTF8.GetString(data);
    }
}
