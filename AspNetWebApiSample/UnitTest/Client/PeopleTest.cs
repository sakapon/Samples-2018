using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleWebApi.Models;

namespace UnitTest.Client
{
    [TestClass]
    public class PeopleTest
    {
        [TestMethod]
        async public Task GetAll()
        {
            for (int i = 0; i < 3; i++)
                await HttpHelper.PostAsJsonAsync("api/people", new Person { Id = -1, Name = $"Person {i}" });

            var result = await HttpHelper.GetAsync<Person[]>("api/people");
            Assert.AreEqual(3, result.Length);

            foreach (var item in result)
                await HttpHelper.DeleteAsync($"api/people/{item.Id}");
        }

        [TestMethod]
        async public Task Get()
        {
            var person = await HttpHelper.PostAsJsonAsync<Person>("api/people", new Person { Name = "GetPerson" });

            var result = await HttpHelper.GetAsync<Person>($"api/people/{person.Id}");
            Assert.AreEqual("GetPerson", result.Name);

            await HttpHelper.DeleteAsync($"api/people/{person.Id}");
        }

        [TestMethod]
        async public Task Put()
        {
            var person = await HttpHelper.PostAsJsonAsync<Person>("api/people", new Person { Name = "GetPerson" });

            person.Name = "PutPerson";
            await HttpHelper.PutAsJsonAsync<Person>($"api/people/{person.Id}", person);

            var result = await HttpHelper.GetAsync<Person>($"api/people/{person.Id}");
            Assert.AreEqual("PutPerson", result.Name);

            await HttpHelper.DeleteAsync($"api/people/{person.Id}");
        }
    }
}
