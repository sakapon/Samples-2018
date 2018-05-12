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

        public static byte[] ToBytes(this DateTime dateTime)
        {
            var ticks = dateTime.Ticks;

            var b = new byte[8];
            b[0] = (byte)(ticks >> 56);
            b[1] = (byte)(ticks >> 48);
            b[2] = (byte)(ticks >> 40);
            b[3] = (byte)(ticks >> 32);
            b[4] = (byte)(ticks >> 24);
            b[5] = (byte)(ticks >> 16);
            b[6] = (byte)(ticks >> 8);
            b[7] = (byte)ticks;
            return b;
        }
    }
}
