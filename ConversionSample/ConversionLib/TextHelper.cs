using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ConversionLib
{
    public static class TextHelper
    {
        public static byte[] EncodeText(this string text, Encoding encoding)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            return encoding.GetBytes(text);
        }

        public static string DecodeText(this byte[] binary, Encoding encoding)
        {
            if (binary == null) throw new ArgumentNullException(nameof(binary));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            return encoding.GetString(binary);
        }

        public static byte[] FromBase64String(this string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return Convert.FromBase64String(text);
        }

        public static string ToBase64String(this byte[] binary)
        {
            if (binary == null) throw new ArgumentNullException(nameof(binary));

            return Convert.ToBase64String(binary);
        }

        public static byte[] FromHexString(this string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (text.Length % 2 != 0) throw new FormatException("The length of the input text must be even.");

            return Enumerable.Range(0, text.Length / 2)
                .Select(i => text.Substring(2 * i, 2))
                .Select(s => byte.Parse(s, NumberStyles.HexNumber))
                .ToArray();
        }

        public static string ToHexString(this byte[] binary, bool uppercase = false)
        {
            if (binary == null) throw new ArgumentNullException(nameof(binary));

            var format = uppercase ? "X2" : "x2";
            return binary
                .Select(b => b.ToString(format))
                .ConcatStrings();
        }

        public static string ConcatStrings(this IEnumerable<string> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return string.Concat(source);
        }

        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return string.Join(separator, source);
        }

        public static string ToIso8601String(this DateTime dateTime)
        {
            return dateTime.ToString("O", CultureInfo.InvariantCulture);
        }

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
