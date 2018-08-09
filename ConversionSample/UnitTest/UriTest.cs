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
            Assert.AreEqual("%20%21%2D%E3%81%82", " !-あ".PercentEncode());
        }

        [TestMethod]
        public void EscapeDataString_RFC3986()
        {
            Assert.AreEqual(RFC3986_UnreservedChars, Uri.EscapeDataString(RFC3986_UnreservedChars));
            Assert.AreEqual(UriHelper.PercentEncode(RFC3986_ReservedChars), Uri.EscapeDataString(RFC3986_ReservedChars));
            Assert.AreEqual(UriHelper.PercentEncode(RFC3986_OtherChars), Uri.EscapeDataString(RFC3986_OtherChars));
            Assert.AreEqual(UriHelper.PercentEncode("あ"), Uri.EscapeDataString("あ"));
        }

        [TestMethod]
        public void EscapeUriString_RFC3986()
        {
            var domain = "https://abc.xyz/";
            Assert.AreEqual(domain + RFC3986_UnreservedChars, Uri.EscapeUriString(domain + RFC3986_UnreservedChars));
            Assert.AreEqual(domain + RFC3986_ReservedChars, Uri.EscapeUriString(domain + RFC3986_ReservedChars));
            Assert.AreEqual(domain + UriHelper.PercentEncode(RFC3986_OtherChars), Uri.EscapeUriString(domain + RFC3986_OtherChars));
            Assert.AreEqual(domain + UriHelper.PercentEncode("あ"), Uri.EscapeUriString(domain + "あ"));

            // クエリ文字列も同様に変換されます。application/x-www-form-urlencoded には変換されません。
            Assert.AreEqual(domain + "?q=%5C", Uri.EscapeUriString(domain + "?q=\\"));
            Assert.AreEqual(domain + "?q=Hello~,%20World!", Uri.EscapeUriString(domain + "?q=Hello~, World!"));

            Assert.AreEqual(domain + "%252", Uri.EscapeUriString(domain + "%2"));
            Assert.AreEqual(domain + "%2525", Uri.EscapeUriString(domain + "%25"));
            Assert.AreEqual(domain + "?q=%252", Uri.EscapeUriString(domain + "?q=%2"));
            Assert.AreEqual(domain + "?q=%2525", Uri.EscapeUriString(domain + "?q=%25"));
        }

        [TestMethod]
        public void Uri_Segment_RFC3986()
        {
            var domain = "https://abc.xyz/";
            Assert.AreEqual(domain + RFC3986_UnreservedChars, new Uri(domain + RFC3986_UnreservedChars).AbsoluteUri);
            Assert.AreEqual(domain + RFC3986_ReservedChars, new Uri(domain + RFC3986_ReservedChars).AbsoluteUri);
            // Uri クラスでは \ が / に変換されます。
            Assert.AreEqual($"{domain}{UriHelper.PercentEncode(" \"%<>")}/{UriHelper.PercentEncode("^`{|}")}", new Uri(domain + RFC3986_OtherChars).AbsoluteUri);
            Assert.AreEqual(domain + UriHelper.PercentEncode("あ"), new Uri(domain + "あ").AbsoluteUri);

            // Uri クラスでは %XX の形式になっているかどうかで扱いが異なります。
            Assert.AreEqual(domain + "%252", new Uri(domain + "%2").AbsoluteUri);
            Assert.AreEqual(domain + "%25", new Uri(domain + "%25").AbsoluteUri);
        }

        [TestMethod]
        public void Uri_Query_RFC3986()
        {
            // クエリ文字列も EscapeUriString メソッドと同様に変換されます。application/x-www-form-urlencoded には変換されません。
            var domain = "https://abc.xyz/?q=";
            Assert.AreEqual(domain + RFC3986_UnreservedChars, new Uri(domain + RFC3986_UnreservedChars).AbsoluteUri);
            Assert.AreEqual(domain + RFC3986_ReservedChars, new Uri(domain + RFC3986_ReservedChars).AbsoluteUri);
            Assert.AreEqual(domain + UriHelper.PercentEncode(RFC3986_OtherChars), new Uri(domain + RFC3986_OtherChars).AbsoluteUri);
            Assert.AreEqual(domain + UriHelper.PercentEncode("あ"), new Uri(domain + "あ").AbsoluteUri);

            // Uri クラスでは %XX の形式になっているかどうかで扱いが異なります。
            Assert.AreEqual(domain + "%252", new Uri(domain + "%2").AbsoluteUri);
            Assert.AreEqual(domain + "%25", new Uri(domain + "%25").AbsoluteUri);
        }

        [TestMethod]
        public void FormUrlEncodedContent_RFC3986()
        {
            var expected = $"unreserved={RFC3986_UnreservedChars}&reserved={RFC3986_ReservedChars.PercentEncode()}&others={RFC3986_OtherChars.PercentEncode().Replace("%20", "+")}";

            var data = new Dictionary<string, string>
            {
                { "unreserved", RFC3986_UnreservedChars },
                { "reserved", RFC3986_ReservedChars },
                { "others", RFC3986_OtherChars },
            };
            var actual = UriHelper.ToFormUrlEncoded(data);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UrlEncode_1()
        {
            Console.WriteLine(SymbolChars);

            // space and "%<>\^`{|}
            UrlEncodeTest(Uri.EscapeUriString);
            // space and !"#$%&'()*+,/:;<=>?@[\]^`{|}
            UrlEncodeTest(Uri.EscapeDataString);
        }

        [TestMethod]
        public void UrlEncodeForForm_1()
        {
            Console.WriteLine(SymbolChars);

            // space and "#$%&'+,/:;<=>?@[\]^`{|}~
            UrlEncodeTest(NetWebUtility.UrlEncode);
            // space and "#$%&'+,/:;<=>?@[\]^`{|}~
            UrlEncodeTest(s => WebHttpUtility.UrlEncode(s).ToUpperInvariant());
            // space and !"#$%&'()*+,/:;<=>?@[\]^`{|}
            UrlEncodeTest(UriHelper.UrlEncodeForForm);
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
