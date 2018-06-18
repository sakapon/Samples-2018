using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class UriQueryTest
    {
        const string Alphanumerics = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string Rfc3986_UnreservedChars = "-._~"; // 4 symbols
        const string Rfc3986_ReservedChars = "!#$&'()*+,/:;=?@[]"; // 18 symbols
        const string Rfc3986_OtherChars = " \"%<>\\^`{|}"; // 11 symbols

        [TestMethod]
        public void Get_Uri()
        {
            void Test(string id)
            {
                var uri = "api/uriquery/{0}".FormatUri(id);
                Console.WriteLine(uri);

                var result = HttpHelper.GetAsync<string>(uri).GetAwaiter().GetResult();
                Assert.AreEqual(id, result);
            }

            // Percent Encoding しても使用できない文字があります。
            Test(Alphanumerics);
            Test(Rfc3986_UnreservedChars);
            Test(Rfc3986_ReservedChars);
            Test(Rfc3986_OtherChars);
            Test("あ");
        }

        [TestMethod]
        public void Get_Query()
        {
            void Test(string id)
            {
                var uri = "api/uriquery".AddQuery(new { id });
                Console.WriteLine(uri);

                var result = HttpHelper.GetAsync<string>(uri).GetAwaiter().GetResult();
                Assert.AreEqual(id, result);
            }

            Test(Alphanumerics);
            Test(Rfc3986_UnreservedChars);
            Test(Rfc3986_ReservedChars);
            Test(Rfc3986_OtherChars);
            Test("あ");
        }

        [TestMethod]
        public void Post()
        {
            void Test(string name)
            {
                var result = HttpHelper.PostAsFormAsync<string>("api/uriquery", new { name }).GetAwaiter().GetResult();
                Assert.AreEqual(name, result);
            }

            Test(Alphanumerics);
            Test(Rfc3986_UnreservedChars);
            Test(Rfc3986_ReservedChars);
            Test(Rfc3986_OtherChars);
            Test("あ");
        }
    }
}
