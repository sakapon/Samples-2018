using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTest.Client
{
    public static class HttpHelper
    {
        static readonly Uri BaseUri = new Uri("http://localhost:1961/");

        async public static Task<T> GetAsync<T>(string uri)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.GetAsync(uri);
                return await response.Content.ReadAsAsync<T>();
            }
        }

        public static Task<T> GetAsync<T>(string uri, Dictionary<string, object> parameters) =>
            GetAsync<T>(uri, parameters.ToDictionary(p => p.Key, p => p.Value?.ToString()));

        async public static Task<T> GetAsync<T>(string uri, Dictionary<string, string> parameters)
        {
            using (var content = new FormUrlEncodedContent(parameters))
            {
                var query = await content.ReadAsStringAsync();
                return await GetAsync<T>($"{uri}?{query}");
            }
        }
    }
}
