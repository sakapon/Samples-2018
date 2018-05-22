using System;
using System.IO;
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

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void Encrypt_Wrong()
        {
            var data = "P@ssw0rd";
            var key = SymmetricEncryption.GenerateKeyBase64();
            var key2 = SymmetricEncryption.GenerateKeyBase64();

            var encrypted = SymmetricEncryption.Encrypt(data, key);
            var decrypted2 = SymmetricEncryption.Decrypt(encrypted, key2);
        }

        [TestMethod]
        public void Encrypt_Old()
        {
            var data = "P@ssw0rd";

            var key = SymmetricEncryption.GenerateKeyBase64();
            var encrypted = SymmetricEncryption_Old.Encrypt(data, key);
            var decrypted = SymmetricEncryption_Old.Decrypt(encrypted, key);

            Console.WriteLine(encrypted);
            Assert.AreEqual(data, decrypted);
        }

        [TestMethod]
        public void Encrypt_Old_Equals()
        {
            var data = "P@ssw0rd";
            var key = SymmetricEncryption.GenerateKeyBase64();

            var encrypted = SymmetricEncryption.Encrypt(data, key);
            var encrypted2 = SymmetricEncryption_Old.Encrypt(data, key);

            Assert.AreEqual(encrypted, encrypted2);
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
            var key = SymmetricEncryption.GenerateKeyBase64();
            var encryptedStream = new MemoryStream();
            var decryptedStream = new MemoryStream();

            SymmetricEncryption.Encrypt(new MemoryStream(data), encryptedStream, key);
            var encrypted = encryptedStream.ToArray();
            SymmetricEncryption.Decrypt(new MemoryStream(encrypted), decryptedStream, key);
            var decrypted = decryptedStream.ToArray();

            if (dataSize <= 1024)
                Console.WriteLine(Convert.ToBase64String(encrypted));
            CollectionAssert.AreEqual(data, decrypted);
        }
    }
}
