using System;
using System.Collections.Generic;
using System.Linq;
using ConversionLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebUtility = System.Net.WebUtility;
using WebHttpUtility = System.Web.HttpUtility;

namespace UnitTest
{
    [TestClass]
    public class UriTest
    {
        // 33 symbols
        static readonly string AsciiString = string.Concat(Enumerable.Range(32, 127 - 32).Select(i => (char)(short)i).Where(c => !char.IsLetter(c) && !char.IsNumber(c)));
        static readonly short[] PercentEncodeSet = new short[] { 32, 34, 35, 60, 62, 96, 123, 125, 126 };

        [TestMethod]
        public void EscapeUriString_1()
        {
            Console.WriteLine(AsciiString);

            UriEncodeTest(Uri.EscapeUriString);
            UriEncodeTest(NetWebUtility.UrlEncode);
            UriEncodeTest(s => WebHttpUtility.UrlEncode(s).ToUpperInvariant());
            UriEncodeTest(Uri.EscapeDataString);
        }

        static void UriEncodeTest(Func<string, string> urlEncode)
        {
            var escaped = AsciiString
                .Select(c => new { c = c.ToString(), e = urlEncode(c.ToString()) })
                .Where(_ => _.c != _.e)
                .ToArray();

            Console.WriteLine(escaped.Select(_ => _.c).ConcatStrings());
            Console.WriteLine(escaped.Select(_ => _.e).ConcatStrings());
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
