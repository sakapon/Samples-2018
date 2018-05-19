using System;
using System.Security.Cryptography;
using ConversionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Cryptography
{
    [TestClass]
    public class RandomHelperTest
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
            var length = 64;
            var actual = RandomHelper.GenerateBase64(length);
            Console.WriteLine(actual);
            Assert.AreEqual(length, Convert.FromBase64String(actual).Length);
        }
    }
}
