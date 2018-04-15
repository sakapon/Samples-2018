using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class ColorsTest
    {
        async public static Task<byte[]> GetPngAsync(string uri)
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                var response = await http.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                Assert.AreEqual("image/png", response.Content.Headers.ContentType.MediaType);
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        async public static Task Test_NotFound(string uri)
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                var response = await http.GetAsync(uri);
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [TestMethod]
        async public Task GetImage_1()
        {
            var result = await GetPngAsync("api/Colors/Orange.png");

            Console.WriteLine(result.Length);
        }

        [TestMethod]
        async public Task GetImage_2()
        {
            var result = await GetPngAsync("api/Colors/3399FF.png?w=900&h=500");

            Console.WriteLine(result.Length);
        }

        [TestMethod]
        async public Task GetImage_NotFound_1()
        {
            await Test_NotFound("api/Colors/abc.png");
        }

        [TestMethod]
        async public Task GetImage_NotFound_2()
        {
            await Test_NotFound("api/Colors/abc");
        }
    }
}
