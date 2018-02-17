using System;
using ConversionLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ValueHelperTest
    {
        [TestMethod]
        public void To_1()
        {
            Assert.AreEqual(123, "123".To<int>());
        }
    }
}
