using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class GlobalTest
    {
        [TestMethod]
        async public Task Echo_Cors()
        {
            var i = 234;

            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                http.DefaultRequestHeaders.Add("Origin", "http://localhost:8080");
                var response = await http.GetAsync($"api/Random/Echo/{i}");
                response.EnsureSuccessStatusCode();

                var allowOrigin = response.Headers.GetValues("Access-Control-Allow-Origin").ToArray();
                CollectionAssert.AreEqual(new[] { "*" }, allowOrigin);
                var result = await response.Content.ReadAsAsync<int>();
                Assert.AreEqual(i, result);
            }
        }

        async static Task Echo_ContentType(int i, string[] acceptMediaTypes, string expectedMediaType)
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                foreach (var mediaType in acceptMediaTypes)
                    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                var response = await http.GetAsync($"api/Random/Echo/{i}");
                response.EnsureSuccessStatusCode();

                Assert.AreEqual(expectedMediaType, response.Content.Headers.ContentType.MediaType);
                var result = await response.Content.ReadAsAsync<int>();
                Assert.AreEqual(i, result);
            }
        }

        [TestMethod]
        async public Task Echo_Json()
        {
            await Echo_ContentType(12, new[] { "application/json" }, "application/json");
        }

        [TestMethod]
        async public Task Echo_Xml()
        {
            await Echo_ContentType(-12, new[] { "application/xml" }, "application/xml");
        }

        [TestMethod]
        async public Task Echo_Html()
        {
            await Echo_ContentType(int.MaxValue, new[] { "text/html" }, "application/json");
        }

        [TestMethod]
        async public Task Echo_Chrome()
        {
            await Echo_ContentType(1234, new[] { "text/html", "application/xml" }, "application/json");
        }
    }
}
