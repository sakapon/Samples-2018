using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebUtility = System.Net.WebUtility;
using WebHttpUtility = System.Web.HttpUtility;

namespace UnitTest
{
    [TestClass]
    public class UriTest
    {
        static readonly string AsciiString = string.Concat(Enumerable.Range(32, 127 - 32).Select(i => (char)(short)i));
        static readonly short[] PercentEncodeSet = new short[] { 32, 34, 35, 60, 62, 96, 123, 125, 126 };

        [TestMethod]
        public void EscapeUriString_1()
        {
            Console.WriteLine(AsciiString);
            Console.WriteLine(Uri.EscapeUriString(AsciiString));
            Console.WriteLine(NetWebUtility.UrlEncode(AsciiString));
            Console.WriteLine(WebHttpUtility.UrlEncode(AsciiString));
            Console.WriteLine(Uri.EscapeDataString(AsciiString));
        }

        [TestMethod]
        public void EscapeUriString_2()
        {
            Assert.AreEqual("%E3%81%82", Uri.EscapeUriString("あ"));
        }

        [TestMethod]
        public void HtmlEncode_1()
        {
            Console.WriteLine(AsciiString);
            Console.WriteLine(NetWebUtility.HtmlEncode(AsciiString));
            Console.WriteLine(WebHttpUtility.HtmlEncode(AsciiString));
        }
    }
}
