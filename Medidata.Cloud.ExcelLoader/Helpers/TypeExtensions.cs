﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using ImpromptuInterface;

namespace Medidata.Cloud.ExcelLoader.Helpers
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyDescriptor> GetPropertyDescriptors(this Type type)
        {
            return TypeDescriptor.GetProperties(type).OfType<PropertyDescriptor>();
        }

        public static PropertyDescriptor GetPropertyDescriptor(this Type type, string propertyName)
        {
            return GetPropertyDescriptors(type).FirstOrDefault(x => x.Name == propertyName);
        }

        public static object GetPropertyValue(this object target, string propName)
        {
            try
            {
                return Impromptu.InvokeGet(target, propName);
            }
            catch
            {
                return null;
            }
        }
    }


    public interface IExtraProperty
    {
        ExpandoObject ExtraProperties { get; }
    }

    public static class ObjectExtensions
    {
        public static IList<T> AddSimilarShape<T>(this IList<T> target, params object[] items) where T: class
        {
            var newItems = items.Select(ActAs<T>);
            target.AddRange(newItems);
            return target;
        } 

        internal static T ActAs<T>(this object target) where T : class
        {
            if (target == null) return null;
            var expando = target as ExpandoObject;

            return expando == null ? target.ActLike<T>() : ActWithExtraProperties<T>(expando);
        }

        private static T ActWithExtraProperties<T>(ExpandoObject target) where T : class
        {
            IDictionary<string, object> targetDic = target;

            dynamic expando = new ExpandoObject();
            expando.ExtraProperties = new ExpandoObject();

            IDictionary<string, object> expandoDic = expando;
            IDictionary<string, object> extraPropPropertyDic = expando.ExtraProperties;

            var typeProps = typeof(T).GetPropertyDescriptors().ToList();
            foreach (var typeProp in typeProps)
            {
                var propValue = targetDic[typeProp.Name];
                expandoDic.Add(typeProp.Name, propValue);
            }
            var props = targetDic.Keys;
            var extraPropNames = props.Except(typeProps.Select(x => x.Name));

            foreach (var extraPropName in extraPropNames)
            {
                extraPropPropertyDic.Add(extraPropName, targetDic[extraPropName]);
            }

            T actor = Impromptu.ActLike(expando, typeof(T), typeof(IExtraProperty));
            return actor;
        }
    }
}