using System;
using System.IO;
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

        [TestMethod]
        public void Encrypt_Stream_Short()
        {
            Encrypt_Stream(100);
        }

        [TestMethod]
        public void Encrypt_Stream_Long()
        {
            Encrypt_Stream(10000);
            Encrypt_Stream(999999);
        }

        static void Encrypt_Stream(int dataSize)
        {
            var data = CryptoHelper.GenerateBytes(dataSize);
            var keys = HybridEncryption.GenerateKeys();
            var encryptedStream = new MemoryStream();
            var decryptedStream = new MemoryStream();

            HybridEncryption.Encrypt(new MemoryStream(data), encryptedStream, keys.PublicKey);
            var encrypted = encryptedStream.ToArray();
            HybridEncryption.Decrypt(new MemoryStream(encrypted), decryptedStream, keys.KeysPair);
            var decrypted = decryptedStream.ToArray();

            if (dataSize <= 1024)
                Console.WriteLine(Convert.ToBase64String(encrypted));
            CollectionAssert.AreEqual(data, decrypted);
        }
    }
}
