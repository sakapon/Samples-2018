using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class ValuesTest
    {
        [TestMethod]
        async public Task Get()
        {
            using (var http = new HttpClient { BaseAddress = new Uri("http://localhost:1961/") })
            {
                var response = await http.GetAsync("api/values");
                var result = await response.Content.ReadAsAsync<string[]>();

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Length);
                Assert.AreEqual("value1", result[0]);
                Assert.AreEqual("value2", result[1]);
            }
        }
    }
}
