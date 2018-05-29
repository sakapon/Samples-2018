using System;
using System.Collections.Generic;
using System.Linq;

namespace ConversionLib
{
    public static class ValueHelper
    {
        public static T To<T>(this object value) => (T)value.ConvertType(typeof(T));

        public static object ConvertType(this object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return value?.ConvertType(type.GenericTypeArguments[0]);
            }
            if (type.IsEnum)
            {
                if (value == null) throw new InvalidCastException("The null value can not be converted to a value type.");

                return Enum.Parse(type, value.ToString(), true);
            }
            if (type == typeof(TimeSpan))
            {
                if (value == null) throw new InvalidCastException("The null value can not be converted to a value type.");

                return TimeSpan.Parse(value.ToString());
            }
            if (type == typeof(DateTimeOffset))
            {
                if (value == null) throw new InvalidCastException("The null value can not be converted to a value type.");

                return DateTimeOffset.Parse(value.ToString());
            }
            return Convert.ChangeType(value, type);
        }

        public static byte[] ToBytes(this DateTime dateTime) =>
            dateTime.Ticks.ToBytes();

        public static byte[] ToBytes(this long value)
        {
            var b = new byte[8];
            b[0] = (byte)(value >> 56);
            b[1] = (byte)(value >> 48);
            b[2] = (byte)(value >> 40);
            b[3] = (byte)(value >> 32);
            b[4] = (byte)(value >> 24);
            b[5] = (byte)(value >> 16);
            b[6] = (byte)(value >> 8);
            b[7] = (byte)value;
            return b;
        }
    }
}
