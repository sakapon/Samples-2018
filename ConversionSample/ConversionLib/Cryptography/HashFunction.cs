using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ConversionLib.Cryptography
{
    public abstract class HashFunctionBase
    {
        public abstract int HashSize { get; }
        public abstract int SaltSize { get; }

        public abstract byte[] GenerateHash(byte[] data);
        public abstract byte[] GenerateHash(byte[] data, byte[] salt);
    }

    public class SHA256Hash : HashFunctionBase
    {
        public override int HashSize { get; } = 256 / 8;
        public override int SaltSize { get; } = 512 / 8;

        public override byte[] GenerateHash(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            using (var algorithm = new SHA256Managed())
            {
                // The result has 256 bits.
                return algorithm.ComputeHash(data);
            }
        }

        public override byte[] GenerateHash(byte[] data, byte[] salt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            using (var algorithm = new HMACSHA256(salt))
            {
                // The result has 256 bits.
                return algorithm.ComputeHash(data);
            }
        }
    }

    public class Rfc2898Hash : HashFunctionBase
    {
        const int Iterations = 10000;

        public override int HashSize { get; } = 256 / 8;
        public override int SaltSize { get; } = 128 / 8;

        public override byte[] GenerateHash(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            using (var algorithm = new Rfc2898DeriveBytes(data, new byte[SaltSize], Iterations))
            {
                return algorithm.GetBytes(HashSize);
            }
        }

        public override byte[] GenerateHash(byte[] data, byte[] salt)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            using (var algorithm = new Rfc2898DeriveBytes(data, salt, Iterations))
            {
                return algorithm.GetBytes(HashSize);
            }
        }
    }
}
