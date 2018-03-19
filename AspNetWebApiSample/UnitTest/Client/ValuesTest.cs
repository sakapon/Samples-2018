using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Client
{
    [TestClass]
    public class ValuesTest
    {
        [TestMethod]
        async public Task GetAll()
        {
            var result = await HttpHelper.GetAsync<string[]>("api/values");

            var expected = new[] { "value0", "value1" };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        async public Task Get_1()
        {
            var result = await HttpHelper.GetAsync<string>("api/values/1");

            Assert.AreEqual("value1", result);
        }

        [TestMethod]
        async public Task Get_2()
        {
            var result = await HttpHelper.GetAsync<string>("api/values", new Dictionary<string, object> { { "id", 1 } });

            Assert.AreEqual("value1", result);
        }
    }
}
