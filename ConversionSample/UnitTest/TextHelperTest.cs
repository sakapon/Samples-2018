using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class TextHelperTest
    {
        [TestMethod]
        public void Convert_Base()
        {
            var data = 123456;
            var @base = 16;
            var text = Convert.ToString(data, @base);
            var data2 = Convert.ToInt32(text, @base);
            Assert.AreEqual(data2, data);
        }
    }
}
