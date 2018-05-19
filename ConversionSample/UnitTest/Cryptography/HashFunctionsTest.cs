using System;
using System.Security.Cryptography;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class HashFunctionsTest
    {
        [TestMethod]
        public void Create()
        {
            var algorithm = HashAlgorithm.Create();
            Assert.IsInstanceOfType(algorithm, typeof(SHA1CryptoServiceProvider));
            Assert.AreEqual(160, algorithm.HashSize); // in bits
        }

        [TestMethod]
        public void Create_SHA256()
        {
            var algorithm = SHA256.Create();
            Assert.IsInstanceOfType(algorithm, typeof(SHA256Managed));
            Assert.AreEqual(256, algorithm.HashSize); // in bits
        }

        [TestMethod]
        public void Create_KeyedHashAlgorithm()
        {
            var algorithm = KeyedHashAlgorithm.Create();
            Assert.IsInstanceOfType(algorithm, typeof(HMACSHA1));
            Assert.AreEqual(160, algorithm.HashSize); // in bits
            Assert.AreEqual(64, algorithm.Key.Length); // in Bytes
        }

        [TestMethod]
        public void Create_HMACSHA256()
        {
            var algorithm = new HMACSHA256();
            Assert.AreEqual(256, algorithm.HashSize); // in bits
            Assert.AreEqual(64, algorithm.Key.Length); // in Bytes
        }

        [TestMethod]
        public void GenerateHash_HMACSHA256()
        {
            HashFunctions.Algorithm = new HMACSHA256Hash();
            GenerateHash();
        }

        [TestMethod]
        public void GenerateHash_Rfc2898()
        {
            HashFunctions.Algorithm = new Rfc2898Hash();
            GenerateHash();
        }

        [TestMethod]
        public void GenerateHashWithSalt_HMACSHA256Hash()
        {
            HashFunctions.Algorithm = new HMACSHA256Hash();
            GenerateHashWithSalt();
        }

        [TestMethod]
        public void GenerateHashWithSalt_Rfc2898Hash()
        {
            HashFunctions.Algorithm = new Rfc2898Hash();
            GenerateHashWithSalt();
        }

        static void GenerateHash()
        {
            var data = "P@ssw0rd";
            var salt = HashFunctions.GenerateSaltString();
            var hash = HashFunctions.GenerateHash(data, salt);
            Console.WriteLine(hash);

            var data2 = "P@ssw1rd";
            var salt2 = HashFunctions.GenerateSaltString();
            var hash2 = Convert.ToBase64String(HashFunctions.GenerateBytes(HashFunctions.HashLength));

            Assert.IsTrue(HashFunctions.VerifyByHash(data, salt, hash));
            Assert.IsFalse(HashFunctions.VerifyByHash(data2, salt, hash));
            Assert.IsFalse(HashFunctions.VerifyByHash(data, salt2, hash));
            Assert.IsFalse(HashFunctions.VerifyByHash(data, salt, hash2));
        }

        static void GenerateHashWithSalt()
        {
            var data = "P@ssw0rd";
            var hash = HashFunctions.GenerateHashWithSalt(data);
            Console.WriteLine(hash);

            var data2 = "P@ssw1rd";
            var hash2 = Convert.ToBase64String(HashFunctions.GenerateBytes(HashFunctions.SaltLength + HashFunctions.HashLength));

            Assert.IsTrue(HashFunctions.VerifyByHashWithSalt(data, hash));
            Assert.IsFalse(HashFunctions.VerifyByHashWithSalt(data2, hash));
            Assert.IsFalse(HashFunctions.VerifyByHashWithSalt(data, hash2));
        }
    }
}
