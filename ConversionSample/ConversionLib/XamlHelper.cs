using System;
using System.Text;
using System.Xaml;

namespace ConversionLib
{
    public static class XamlHelper
    {
        public static byte[] SerializeByXaml(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return XamlServices.Save(obj).EncodeText(Encoding.UTF8);
        }

        public static T DeserializeByXaml<T>(this byte[] binary)
        {
            if (binary == null) throw new ArgumentNullException(nameof(binary));

            return (T)XamlServices.Parse(binary.DecodeText(Encoding.UTF8));
        }
    }
}
