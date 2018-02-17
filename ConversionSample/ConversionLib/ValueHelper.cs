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
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return value?.ConvertType(type.GenericTypeArguments[0]);
            }
            return Convert.ChangeType(value, type);
        }
    }
}
