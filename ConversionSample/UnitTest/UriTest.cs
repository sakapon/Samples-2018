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
        const string RFC3986_UnreservedChars = "-._~"; // 4 symbols
        const string RFC3986_ReservedChars = "!#$&'()*+,/:;=?@[]"; // 18 symbols
        const string RFC3986_OtherChars = " \"%<>\\^`{|}"; // 11 symbols
        const string UrlStandard_PercentEncodeSet = " \"#<>`{}~";

        [TestMethod]
        public void PercentEncode()
        {
            Assert.AreEqual("%20%21%2D%E3%81%82", TextHelper.PercentEncode(" !-あ"));
        }

        [TestMethod]
        public void EscapeDataString_RFC3986()
        {
            Assert.AreEqual(RFC3986_UnreservedChars, Uri.EscapeDataString(RFC3986_UnreservedChars));
            Assert.AreEqual(TextHelper.PercentEncode(RFC3986_ReservedChars), Uri.EscapeDataString(RFC3986_ReservedChars));
            Assert.AreEqual(TextHelper.PercentEncode(RFC3986_OtherChars), Uri.EscapeDataString(RFC3986_OtherChars));
            Assert.AreEqual(TextHelper.PercentEncode("あ"), Uri.EscapeDataString("あ"));
        }

        [TestMethod]
        public void EscapeUriString_RFC3986()
        {
            var domain = "https://abc.xyz/";
            Assert.AreEqual(domain + RFC3986_UnreservedChars, Uri.EscapeUriString(domain + RFC3986_UnreservedChars));
            Assert.AreEqual(domain + RFC3986_ReservedChars, Uri.EscapeUriString(domain + RFC3986_ReservedChars));
            Assert.AreEqual(domain + TextHelper.PercentEncode(RFC3986_OtherChars), Uri.EscapeUriString(domain + RFC3986_OtherChars));
            Assert.AreEqual(domain + TextHelper.PercentEncode("あ"), Uri.EscapeUriString(domain + "あ"));
        }

        [TestMethod]
        public void FormUrlEncodedContent_RFC3986()
        {
            var expected = $"unreserved={RFC3986_UnreservedChars}&reserved={TextHelper.PercentEncode(RFC3986_ReservedChars)}&others={TextHelper.PercentEncode(RFC3986_OtherChars).Replace("%20", "+")}";

            var data = new Dictionary<string, string>
            {
                { "unreserved", RFC3986_UnreservedChars },
                { "reserved", RFC3986_ReservedChars },
                { "others", RFC3986_OtherChars },
            };
            var actual = TextHelper.ToFormUrlEncoded(data);

            Assert.AreEqual(expected, actual);
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
        public void HtmlEncode_1()
        {
            Console.WriteLine(SymbolChars);
            Console.WriteLine(NetWebUtility.HtmlEncode(SymbolChars));
            Console.WriteLine(WebHttpUtility.HtmlEncode(SymbolChars));
        }
    }
}
