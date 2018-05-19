using System;
using System.Security.Cryptography;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class SymmetricEncryptionTest
    {
        [TestMethod]
        public void Create()
        {
            var algorithm = SymmetricAlgorithm.Create();
            Assert.IsInstanceOfType(algorithm, typeof(RijndaelManaged));
            Assert.AreEqual(256, algorithm.KeySize); // in bits
            Assert.AreEqual(128, algorithm.BlockSize); // in bits
        }

        [TestMethod]
        public void Encrypt()
        {
            var data = "P@ssw0rd";

            var key = SymmetricEncryption.GenerateKeyBase64();
            var encrypted = SymmetricEncryption.Encrypt(data, key);
            var decrypted = SymmetricEncryption.Decrypt(encrypted, key);

            Console.WriteLine(encrypted);
            Assert.AreEqual(data, decrypted);
        }
    }
}
