using System;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class HashFunctionsTest
    {
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

        static void GenerateHash()
        {
            var data = "P@ssw0rd";
            var salt = HashFunctions.GenerateSaltString();

            var data2 = "P@ssw1rd";
            var salt2 = HashFunctions.GenerateSaltString();

            var hash = HashFunctions.GenerateHash(data, salt);
            Console.WriteLine(hash);

            Assert.IsTrue(HashFunctions.VerifyByHash(data, salt, hash));
            Assert.IsFalse(HashFunctions.VerifyByHash(data2, salt, hash));
            Assert.IsFalse(HashFunctions.VerifyByHash(data, salt2, hash));
        }
    }
}
