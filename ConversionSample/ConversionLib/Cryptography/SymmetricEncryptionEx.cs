using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
