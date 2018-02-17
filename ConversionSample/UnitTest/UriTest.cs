using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UriTest
    {
        static readonly short[] PercentEncodeSet = new short[] { 32, 34, 35, 60, 62, 96, 123, 125, 126 };

        [TestMethod]
        public void EscapeUriString_1()
        {
            var ascii = string.Join("", Enumerable.Range(32, 127 - 32).Select(i => (char)(short)i));
            var ascii2 = Uri.EscapeUriString(ascii);

            Assert.AreEqual("%E3%81%82", Uri.EscapeUriString("あ"));
        }
    }
}
