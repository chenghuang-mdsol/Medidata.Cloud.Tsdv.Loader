using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Medidata.Cloud.ExcelLoader.Helpers
{
    internal static class TypeExtensions
    {
        public static IEnumerable<PropertyDescriptor> GetPropertyDescriptors(this Type type)
        {
            return TypeDescriptor.GetProperties(type).OfType<PropertyDescriptor>();
        }

        public static bool TryGetDynamicFields<T>(this T model, out IList fields)
        {
            Type[] interfaceTypes = typeof(T).GetInterfaces();
            if (interfaceTypes.All(o => o != typeof(IDynamicFields)))
            {
                fields = null;
                return false;
            }
            fields = ((IDynamicFields)model).DynamicFields;
            return true;
        }

        public static bool ContainsDynamicFields(this Type t)
        {
            Type[] interfaceTypes = t.GetInterfaces();
            return interfaceTypes.Any(o => o == typeof (IDynamicFields));
        }
    }
}