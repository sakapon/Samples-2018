using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class RandomTextTest
    {
        async public static Task<string> GetTextAsync(string uri)
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                var response = await http.GetAsync(uri);
                response.EnsureSuccessStatusCode();

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

        [TestMethod]
        async public Task NewBytes2_NotFound()
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                var response = await http.GetAsync("api/NewBytes2/-1");
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

                var error = await response.Content.ReadAsAsync<HttpError>();
                Console.WriteLine(error.Message);
                Console.WriteLine(error.MessageDetail);
            }
        }
    }
}
