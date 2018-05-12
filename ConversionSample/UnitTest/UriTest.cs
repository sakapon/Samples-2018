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
        static readonly string SymbolChars = string.Concat(Enumerable.Range(32, 127 - 32).Select(i => (char)(short)i).Where(c => !char.IsLetter(c) && !char.IsNumber(c)));
        const string RFC3986_UnreservedChars = "-._~";
        static readonly short[] PercentEncodeSet = new short[] { 32, 34, 35, 60, 62, 96, 123, 125, 126 };

        [TestMethod]
        public void EscapeDataString_RFC3986()
        {
            var unchanged = SymbolChars
                .Select(c => c.ToString())
                .Select(c => new { c, e = Uri.EscapeDataString(c) })
                .Where(_ => _.c == _.e)
                .Select(_ => _.c)
                .ConcatStrings();

            Assert.AreEqual(RFC3986_UnreservedChars, unchanged);
        }

        [TestMethod]
        public void EscapeUriString_1()
        {
            Console.WriteLine(SymbolChars);

            UriEncodeTest(Uri.EscapeUriString);
            UriEncodeTest(Uri.EscapeDataString);
        }

        [TestMethod]
        public void UrlEncodeForForm_1()
        {
            Console.WriteLine(SymbolChars);

            UriEncodeTest(NetWebUtility.UrlEncode);
            UriEncodeTest(s => WebHttpUtility.UrlEncode(s).ToUpperInvariant());
            UriEncodeTest(TextHelper.UrlEncodeForForm);
        }

        static void UriEncodeTest(Func<string, string> urlEncode)
        {
            var escaped = SymbolChars
                .Select(c => c.ToString())
                .Select(c => new { c, e = urlEncode(c) })
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
            Console.WriteLine(SymbolChars);
            Console.WriteLine(NetWebUtility.HtmlEncode(SymbolChars));
            Console.WriteLine(WebHttpUtility.HtmlEncode(SymbolChars));
        }
    }
}
