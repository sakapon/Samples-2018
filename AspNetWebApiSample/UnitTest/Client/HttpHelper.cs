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
    }
}
