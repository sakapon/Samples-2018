using System;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class SymmetricEncryptionExTest
    {
        [TestMethod]
        public void EncryptByPassword()
        {
            var data = Convert.ToBase64String(CryptoHelper.GenerateBytes(100));
            var password = "P@ssw0rd";

            var encrypted = SymmetricEncryptionEx.EncryptByPassword(data, password);
            var decrypted = SymmetricEncryptionEx.DecryptByPassword(encrypted, password);

            Console.WriteLine(encrypted);
            Assert.AreEqual(data, decrypted);
        }
    }
}
