using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace Medidata.Cloud.Tsdv.Loader.Extensions
{
    public static class ReflectionExtentions
    {
        public static object GetAttribute<T>(this object obj, string propertyName, bool inherit)
        {
            var pi = obj.GetType().GetProperty(propertyName);
            if (Attribute.IsDefined(pi, typeof (T)))
            {
                return (T) obj.GetType().GetProperty(propertyName).GetCustomAttributes(typeof (T), inherit).First();
            }
            return null;
        }

        public static TV GetAttributeValue<T, TV>(this object obj, string propertyName, bool inherit, string attributePropertyName, TV defaultValue)
        {
            var pi = obj.GetType().GetProperty(propertyName);
            if (!Attribute.IsDefined(pi, typeof (T)))
            {
                return defaultValue;
            }

            T attr = (T)obj.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(T), inherit).First();
            var attrPi =attr.GetType().GetProperty(attributePropertyName);

            if (attrPi == null)
            {
                return defaultValue;
            }

            object result = attrPi.GetValue(attr, null);

            if (result == null)
            {
                return defaultValue;
            }
            return (TV) result;
        }

        public static T GetProperty<T>(this object obj, string propertyName)
        {
            var pi = obj.GetType().GetProperty(propertyName);
            if (pi == null)
            {
                throw new ArgumentException(string.Format("Could not find property {0} in class {1}", propertyName, obj.GetType().FullName));
            }
            object result = pi.GetValue(obj, null);
            if (!(result is T))
            {
                throw new ArgumentException(string.Format("Property {0} is not of Type {1}", propertyName, typeof(T).FullName));
            }
            return (T) result;

        }
    }
}
