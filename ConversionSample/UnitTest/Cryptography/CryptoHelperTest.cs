using System;
using System.Security.Cryptography;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class CryptoHelperTest
    {
        [TestMethod]
        public void Create()
        {
            var algorithm = RandomNumberGenerator.Create();
            Assert.IsInstanceOfType(algorithm, typeof(RNGCryptoServiceProvider));
        }

        [TestMethod]
        public void GenerateBase64()
        {
            var size = 64;
            var actual = CryptoHelper.GenerateBase64(size);
            Console.WriteLine(actual);
            Assert.AreEqual(size, Convert.FromBase64String(actual).Length);
        }
    }
}
