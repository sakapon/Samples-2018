using System;
using System.Text;
using System.Xaml;

namespace ConversionLib
{
    public static class XamlHelper
    {
        public static byte[] Serialize(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            return XamlServices.Save(obj).Encode(Encoding.UTF8);
        }

        public static T Deserialize<T>(this byte[] binary)
        {
            if (binary == null) throw new ArgumentNullException("binary");

            return (T)XamlServices.Parse(binary.Decode(Encoding.UTF8));
        }
    }
}
