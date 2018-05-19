using System;
using System.Security.Cryptography;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class AsymmetricEncryptionTest
    {
        [TestMethod]
        public void Create()
        {
            var algorithm = AsymmetricAlgorithm.Create();
            Assert.IsInstanceOfType(algorithm, typeof(RSACryptoServiceProvider));
            Assert.AreEqual(1024, algorithm.KeySize); // in bits
        }

        [TestMethod]
        public void Encrypt()
        {
            var data = "P@ssw0rd";

            var keys = AsymmetricEncryption.GenerateKeys();
            var encrypted = AsymmetricEncryption.Encrypt(data, keys.PublicKey);
            var decrypted = AsymmetricEncryption.Decrypt(encrypted, keys.KeysPair);

            Console.WriteLine(encrypted);
            Assert.AreEqual(data, decrypted);
        }
    }
}
