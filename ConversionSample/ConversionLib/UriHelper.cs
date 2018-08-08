using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ConversionLib
{
    public static class UriHelper
    {
        public static string PercentEncode(this string value) =>
            value.EncodeText(Encoding.UTF8)
                .Select(b => "%" + b.ToString("X2"))
                .ConcatStrings();

        public static string UrlEncode(this string value) =>
            Uri.EscapeDataString(value ?? "");

        public static string UrlEncodeForForm(this string value) =>
            value.UrlEncode().Replace("%20", "+");

        public static string UrlDecode(this string value) =>
            Uri.UnescapeDataString(value ?? "");

        public static string UrlDecodeForForm(this string value) =>
            (value ?? "").Replace("+", "%20").UrlDecode();

        public static string ToFormUrlEncoded(this IDictionary<string, string> data)
        {
            using (var content = new FormUrlEncodedContent(data))
                return content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public static string AddQuery(this string uri, IDictionary<string, string> data) =>
            $"{uri}?{data.ToFormUrlEncoded()}";
    }
}
