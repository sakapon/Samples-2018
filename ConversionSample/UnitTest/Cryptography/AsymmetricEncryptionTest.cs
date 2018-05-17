using System;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class AsymmetricEncryptionTest
    {
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
