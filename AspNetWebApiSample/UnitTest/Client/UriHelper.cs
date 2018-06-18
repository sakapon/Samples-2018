using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;

namespace UnitTest.Client
{
    public static class UriHelper
    {
        public static string FormatUri(this string format, params object[] args) =>
            string.Format(format, args.Select(o => Uri.EscapeDataString(o?.ToString() ?? "")).ToArray());

        public static string AddQuery(this string uri, object query) =>
            $"{uri}?{query.ToFormUrlEncoded()}";

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
