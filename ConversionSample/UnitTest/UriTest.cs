using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        const string RFC3986_ReservedChars = "!#$&'()*+,/:;=?@[]";
        const string RFC3986_OtherChars = " \"%<>\\^`{|}";
        const string UrlStandard_PercentEncodeSet = " \"#<>`{}~";

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
        public void EscapeDataString_JP()
        {
            Assert.AreEqual("%E3%81%82", Uri.EscapeDataString("あ"));
        }

        [TestMethod]
        public void EscapeUriString_1()
        {
            Console.WriteLine(Uri.EscapeUriString("https://abc.xyz/" + SymbolChars));
        }

        [TestMethod]
        public void UrlEncode_1()
        {
            Console.WriteLine(SymbolChars);

            UrlEncodeTest(Uri.EscapeUriString);
            UrlEncodeTest(Uri.EscapeDataString);
        }

        [TestMethod]
        public void UrlEncodeForForm_1()
        {
            Console.WriteLine(SymbolChars);

            UrlEncodeTest(NetWebUtility.UrlEncode);
            UrlEncodeTest(s => WebHttpUtility.UrlEncode(s).ToUpperInvariant());
            UrlEncodeTest(TextHelper.UrlEncodeForForm);
        }

        static void UrlEncodeTest(Func<string, string> urlEncode)
        {
            var changed = SymbolChars
                .Select(c => c.ToString())
                .Select(c => new { c, e = urlEncode(c) })
                .Where(_ => _.c != _.e)
                .ToArray();

            Console.WriteLine(changed.Select(_ => _.c).ConcatStrings());
            Console.WriteLine(changed.Select(_ => _.e).ConcatStrings());
        }

        [TestMethod]
        public void UrlDecode_1()
        {
            var actual = SymbolChars.UrlEncode().UrlDecode();
            Assert.AreEqual(SymbolChars, actual);
        }

        [TestMethod]
        public void UrlDecodeForForm_1()
        {
            var actual = SymbolChars.UrlEncodeForForm().UrlDecodeForForm();
            Assert.AreEqual(SymbolChars, actual);
        }

        [TestMethod]
        public void FormUrlEncodedContent_1()
        {
            var data = new Dictionary<string, string>
            {
                { "unreserved", RFC3986_UnreservedChars },
                { "reserved", RFC3986_ReservedChars },
                { "others", RFC3986_OtherChars },
            };
            var content = new FormUrlEncodedContent(data);
            var form = content.ReadAsStringAsync().GetAwaiter().GetResult();

            Console.WriteLine(form);
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
