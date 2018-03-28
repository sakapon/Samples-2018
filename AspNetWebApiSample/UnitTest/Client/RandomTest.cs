using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleWebApi.Controllers;

namespace UnitTest.Client
{
    [TestClass]
    public class RandomTest
    {
        [TestMethod]
        async public Task Echo()
        {
            var result = await HttpHelper.GetAsync<int>("api/Random/Echo");

            Assert.AreEqual(123, result);
        }

        [TestMethod]
        async public Task Echo_2()
        {
            var result = await HttpHelper.GetAsync<int>("api/Random/Echo/321");

            Assert.AreEqual(321, result);
        }

        [TestMethod]
        async public Task Echo_Json()
        {
            await Echo_ContentType(12, "application/json", "application/json");
        }

        [TestMethod]
        async public Task Echo_Xml()
        {
            await Echo_ContentType(-12, "application/xml", "application/xml");
        }

        [TestMethod]
        async public Task Echo_Html()
        {
            await Echo_ContentType(1234, "text/html", "application/json");
        }

        async static Task Echo_ContentType(int i, string acceptMediaType, string expectedMediaType)
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptMediaType));
                var response = await http.GetAsync($"api/Random/Echo/{i}");
                response.EnsureSuccessStatusCode();
                Assert.AreEqual(expectedMediaType, response.Content.Headers.ContentType.MediaType);
                var result = await response.Content.ReadAsAsync<int>();
                Assert.AreEqual(i, result);
            }
        }

        [TestMethod]
        async public Task NewInteger1()
        {
            var result = await HttpHelper.GetAsync<int>("api/Random/NewInteger", new RangeInfo { MinValue = 5, MaxValue = 10 });

            Assert.IsTrue(5 <= result && result < 10);
        }

        [TestMethod]
        async public Task NewInteger2_Form()
        {
            var result = await HttpHelper.PostAsFormAsync<int>("api/Random/NewInteger", new RangeInfo { MinValue = 5, MaxValue = 10 });

            Assert.IsTrue(5 <= result && result < 10);
        }

        [TestMethod]
        async public Task NewInteger2_Json()
        {
            var result = await HttpHelper.PostAsJsonAsync<int>("api/Random/NewInteger", new RangeInfo { MinValue = 5, MaxValue = 10 });

            Assert.IsTrue(5 <= result && result < 10);
        }

        [TestMethod]
        async public Task NewInteger1_BadRequest()
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                var response = await http.GetAsync(HttpHelper.AddQuery("api/Random/NewInteger", new RangeInfo { MinValue = 20, MaxValue = 10 }));
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [TestMethod]
        async public Task NewDoubles()
        {
            var result = await HttpHelper.GetAsync<double[]>("api/Random/NewDoubles/10");

            Assert.AreEqual(10, result.Length);
            Assert.IsTrue(result.All(x => 0.0 <= x && x < 1.0));

            foreach (var item in result)
                Console.WriteLine(item);
        }

        [TestMethod]
        async public Task NewDoubles_NotFound()
        {
            using (var http = new HttpClient { BaseAddress = HttpHelper.BaseUri })
            {
                var response = await http.GetAsync("api/Random/NewDoubles/65");
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [TestMethod]
        async public Task NewDateTime_1()
        {
            var result = await HttpHelper.GetAsync<DateTime>($"api/Random/NewDateTime/{DateTime.Now:yyyy-MM-dd}");

            Assert.AreEqual(DateTime.Now.Date, result.Date);
        }

        [TestMethod]
        async public Task NewDateTime_2()
        {
            var result = await HttpHelper.GetAsync<DateTime>($"api/Random/NewDateTime/{DateTime.Now:yyyy/MM/dd}");

            Assert.AreEqual(DateTime.Now.Date, result.Date);
        }

        [TestMethod]
        async public Task NewUuid()
        {
            var result = await HttpHelper.GetAsync<Guid>("api/Random/NewUuid");
            Console.WriteLine(result);
        }

        [TestMethod]
        async public Task NewUuidInfo()
        {
            var result = await HttpHelper.GetAsync<UuidInfo>("api/Random/NewUuidInfo");

            Assert.AreEqual(DateTime.Now.Date, result.Date.Date);
        }
    }
}
