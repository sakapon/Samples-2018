using System;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class HybridEncryptionTest
    {
        [TestMethod]
        public void Encrypt()
        {
            var data = "P@ssw0rd";

            var keys = HybridEncryption.GenerateKeys();
            var encrypted = HybridEncryption.Encrypt(data, keys.PublicKey);
            var decrypted = HybridEncryption.Decrypt(encrypted, keys.KeysPair);

            Console.WriteLine(encrypted);
            Assert.AreEqual(data, decrypted);
        }
    }
}
