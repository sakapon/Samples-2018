using System;
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
            var result = await HttpHelper.GetAsync<string[]>("api/values");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("value1", result[0]);
            Assert.AreEqual("value2", result[1]);
        }
    }
}
