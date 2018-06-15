using System;
using System.Security.Cryptography;

namespace NetStandardLib
{
    public static class RandomHelper
    {
        public static byte[] GenerateBytes(int size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size), size, "The value must be non-negative.");

            var data = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }
            return data;
        }

        public static string GenerateBase64(int size) => Convert.ToBase64String(GenerateBytes(size));
    }
}
