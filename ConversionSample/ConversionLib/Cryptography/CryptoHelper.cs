using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [DebuggerHidden]
        internal static byte[] ToBytes(this string text) => Encoding.UTF8.GetBytes(text);
        [DebuggerHidden]
        internal static string ToText(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

        internal static void Concat(byte[] b1, byte[] b2, out byte[] result)
        {
            result = new byte[b1.Length + b2.Length];
            Buffer.BlockCopy(b1, 0, result, 0, b1.Length);
            Buffer.BlockCopy(b2, 0, result, b1.Length, b2.Length);
        }

        internal static void Separate(out byte[] b1, out byte[] b2, byte[] source, int b1Length)
        {
            b1 = new byte[b1Length];
            b2 = new byte[source.Length - b1Length];
            Buffer.BlockCopy(source, 0, b1, 0, b1.Length);
            Buffer.BlockCopy(source, b1.Length, b2, 0, b2.Length);
        }

        internal static bool ByteArrayEqual(this byte[] b1, byte[] b2)
        {
            if (Equals(b1, b2)) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;

            for (var i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i]) return false;
            return true;
        }
    }
}
