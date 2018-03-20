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
                response.EnsureSuccessStatusCode();
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

        public static Task<T> PostAsFormAsync<T>(string uri, Dictionary<string, object> parameters) =>
            PostAsFormAsync<T>(uri, parameters.ToDictionary(p => p.Key, p => p.Value?.ToString()));

        async public static Task<T> PostAsFormAsync<T>(string uri, Dictionary<string, string> parameters)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            using (var content = new FormUrlEncodedContent(parameters))
            {
                var response = await http.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<T>();
            }
        }

        async public static Task PostAsJsonAsync(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PostAsJsonAsync(uri, value);
                response.EnsureSuccessStatusCode();
            }
        }

        async public static Task<TResult> PostAsJsonAsync<TResult>(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PostAsJsonAsync(uri, value);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<TResult>();
            }
        }

        async public static Task PutAsJsonAsync(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PutAsJsonAsync(uri, value);
                response.EnsureSuccessStatusCode();
            }
        }

        async public static Task<TResult> PutAsJsonAsync<TResult>(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PutAsJsonAsync(uri, value);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<TResult>();
            }
        }

        async public static Task DeleteAsync(string uri)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
            }
        }

        async public static Task<TResult> DeleteAsync<TResult>(string uri)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<TResult>();
            }
        }
    }
}
