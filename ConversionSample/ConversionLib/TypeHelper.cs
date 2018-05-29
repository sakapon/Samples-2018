using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConversionLib
{
    public static class TypeHelper
    {
        public static IDictionary<string, object> GetPropertyValues(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var type = obj.GetType();
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Where(p => p.GetIndexParameters().Length == 0)
                .ToDictionary(p => p.Name, p => p.GetValue(obj));
        }

        public static IDictionary<string, object> GetFieldValues(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var type = obj.GetType();
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(f => f.Name, f => f.GetValue(obj));
        }

        public static IDictionary<string, object> GetPropertyAndFieldValues(this object obj) =>
            obj.GetPropertyValues().DictionaryConcat(obj.GetFieldValues());

        public static IDictionary<TKey, TValue> DictionaryConcat<TKey, TValue>(this IDictionary<TKey, TValue> d1, IDictionary<TKey, TValue> d2) =>
            d1.Concat(d2).ToDictionary(p => p.Key, p => p.Value);
    }
}
