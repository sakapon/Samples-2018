using System;
using System.Security.Cryptography;
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
    }
}
