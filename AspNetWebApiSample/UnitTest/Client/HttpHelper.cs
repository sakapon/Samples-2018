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

        async public static Task PostAsFormAsync(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PostAsync(uri, new FormUrlEncodedContent(value.EnumerateProperties()));
                response.EnsureSuccessStatusCode();
            }
        }

        async public static Task<T> PostAsFormAsync<T>(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PostAsync(uri, new FormUrlEncodedContent(value.EnumerateProperties()));
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

        async public static Task<T> PostAsJsonAsync<T>(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PostAsJsonAsync(uri, value);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<T>();
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

        async public static Task<T> PutAsJsonAsync<T>(string uri, object value)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.PutAsJsonAsync(uri, value);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<T>();
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

        async public static Task<T> DeleteAsync<T>(string uri)
        {
            using (var http = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await http.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<T>();
            }
        }

        public static string AddQuery(string uri, object query) => $"{uri}?{ToFormUrlEncoded(query)}";

        public static string ToFormUrlEncoded(this object value)
        {
            using (var content = new FormUrlEncodedContent(value.EnumerateProperties()))
                return content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public static IEnumerable<KeyValuePair<string, string>> EnumerateProperties(this object value) =>
             TypeDescriptor.GetProperties(value)
                .Cast<PropertyDescriptor>()
                .Select(d => new KeyValuePair<string, string>(d.Name, d.GetValue(value)?.ToString()));
    }
}
