using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = await HttpHelper.GetAsync<int>("api/Random/Echo/321");

            Assert.AreEqual(321, result);
        }

        [TestMethod]
        async public Task NewInteger1()
        {
            var result = await HttpHelper.GetAsync<int>("api/Random/NewInteger", new Dictionary<string, object> { { "minValue", 5 }, { "maxValue", 10 } });

            Assert.IsTrue(5 <= result && result < 10);
        }

        [TestMethod]
        async public Task NewInteger2_Form()
        {
            var result = await HttpHelper.PostAsFormAsync<int>("api/Random/NewInteger", new Dictionary<string, object> { { "minValue", 5 }, { "maxValue", 10 } });

            Assert.IsTrue(5 <= result && result < 10);
        }

        [TestMethod]
        async public Task NewInteger2_Json()
        {
            var result = await HttpHelper.PostAsJsonAsync<RangeInfo, int>("api/Random/NewInteger", new RangeInfo { MinValue = 5, MaxValue = 10 });

            Assert.IsTrue(5 <= result && result < 10);
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
