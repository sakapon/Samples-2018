using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void SHA1_123456()
        {
            var data = "123456";

            using (var algorithm = new SHA1Managed())
            {
                var hash = Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
                Assert.AreEqual("fEqNCco3Yq9h5ZUglD3CZJT4lBs=", hash);
            }
        }

        [TestMethod]
        public void SHA256_Password()
        {
            var data = "P@ssw0rd";

            using (var algorithm = new SHA256Managed())
            {
                var hash = Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
                Assert.AreEqual("sD3fPKLnFKZUjnSV4qA/XoJOqsmDfNfxWcZ7kPtLc0I=", hash);
            }
        }

        [TestMethod]
        public void HMACSHA256_Password()
        {
            var data = "P@ssw0rd";
            var salt = "yV2oWreRPM/+w4PgUsFoLOA2taQxOIb3KFDNebohwKPBt+tvtQnzDhIwPlq4aEGFeu+rIDy7jSRJazst1Io4GQ==";

            using (var algorithm = new HMACSHA256(Convert.FromBase64String(salt)))
            {
                var hash = Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
                Assert.AreEqual("7yxBnMN7/IpBxMzpIrJfPnSBnK5fmUCv5T9hyN54rZk=", hash);
            }
        }

        [TestMethod]
        public void HMACSHA256_Password_WithSalt()
        {
            var data = "P@ssw0rd";
            var saltBase64 = "yV2oWreRPM/+w4PgUsFoLOA2taQxOIb3KFDNebohwKPBt+tvtQnzDhIwPlq4aEGFeu+rIDy7jSRJazst1Io4GQ==";
            var expected = "yV2oWreRPM/+w4PgUsFoLOA2taQxOIb3KFDNebohwKPBt+tvtQnzDhIwPlq4aEGFeu+rIDy7jSRJazst1Io4Ge8sQZzDe/yKQcTM6SKyXz50gZyuX5lAr+U/YcjeeK2Z";

            var salt = Convert.FromBase64String(saltBase64);
            using (var algorithm = new HMACSHA256(salt))
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
                CryptoHelper.Concat(salt, hash, out var hashWithSalt);
                var result = Convert.ToBase64String(hashWithSalt);

                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void BruteForceAttack_SHA256Hash()
        {
            HashFunctions.Algorithm = new SHA256Hash();
            BruteForceAttack();
        }

        [TestMethod]
        public void BruteForceAttack_Rfc2898Hash()
        {
            HashFunctions.Algorithm = new Rfc2898Hash();
            BruteForceAttack();
        }

        static void BruteForceAttack()
        {
            var password = "abc";
            var salt = HashFunctions.GenerateSaltBase64();
            var hash = HashFunctions.GenerateHash(password, salt);
            Console.WriteLine(hash);

            var solved = GetPasswords(password.Length)
                .First(s => HashFunctions.VerifyByHash(s, salt, hash));
            Assert.AreEqual(password, solved);
        }

        const string PasswordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static IEnumerable<string> GetPasswords(int length)
        {
            if (length == 0) return new[] { "" };
            return GetPasswords(length - 1).SelectMany(AddChar);
        }

        static IEnumerable<string> AddChar(string predecessor) =>
            PasswordChars.Select(c => predecessor + c);
    }
}
