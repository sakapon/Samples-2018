using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static Task<T> GetAsync<T>(string uri, object query) =>
            GetAsync<T>(AddQuery(uri, query));

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

        public static string AddQuery(string uri, object query) => $"{uri}?{ToFormUrlEncoded(query)}";

        public static string ToFormUrlEncoded(this object value)
        {
            var properties = TypeDescriptor.GetProperties(value)
                .Cast<PropertyDescriptor>()
                .Select(d => new KeyValuePair<string, string>(d.Name, d.GetValue(value)?.ToString()));

            using (var content = new FormUrlEncodedContent(properties))
                return content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
