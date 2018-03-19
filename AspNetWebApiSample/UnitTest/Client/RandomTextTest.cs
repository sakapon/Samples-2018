using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class RandomTextTest
    {
        static readonly Uri BaseUri = new Uri("http://localhost:1961/");

        async public static Task<string> GetTextAsync(string uri)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.GetAsync(uri);

                Assert.AreEqual("text/plain", response.Content.Headers.ContentType.MediaType);
                return await response.Content.ReadAsStringAsync();
            }
        }

        [TestMethod]
        async public Task NewBytes1()
        {
            var result = await GetTextAsync("api/NewBytes1/10");

            var bytes = result.Split('\n').Select(byte.Parse).ToArray();
            Assert.AreEqual(10, bytes.Length);

            foreach (var item in bytes)
                Console.WriteLine(item);
        }

        [TestMethod]
        async public Task NewBytes2()
        {
            var result = await GetTextAsync("api/NewBytes2/10");

            var bytes = result.Split('\n').Select(byte.Parse).ToArray();
            Assert.AreEqual(10, bytes.Length);

            foreach (var item in bytes)
                Console.WriteLine(item);
        }
    }
}
