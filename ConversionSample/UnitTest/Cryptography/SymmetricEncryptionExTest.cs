using System;
using System.IO;
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

        [TestMethod]
        public void EncryptWithCompression()
        {
            Encrypt_Compression(10000);
            Encrypt_Compression(999999);
        }

        static void Encrypt_Compression(int dataSize)
        {
            var dataText = string.Concat(CryptoHelper.GenerateBytes(dataSize));
            var data = dataText.ToBytes();
            var key = SymmetricEncryption.GenerateKeyBase64();
            var encryptedStream = new MemoryStream();
            var decryptedStream = new MemoryStream();

            SymmetricEncryptionEx.EncryptWithCompression(new MemoryStream(data), encryptedStream, key);
            var encrypted = encryptedStream.ToArray();
            SymmetricEncryptionEx.DecryptWithDecompression(new MemoryStream(encrypted), decryptedStream, key);
            var decrypted = decryptedStream.ToArray();

            Console.WriteLine($"Data: {data.Length} Bytes");
            Console.WriteLine($"Encrypted: {encrypted.Length} Bytes");

            CollectionAssert.AreEqual(data, decrypted);
        }
    }
}
