using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using ImpromptuInterface;

namespace Medidata.Cloud.Tsdv.Loader.ModelConverters
{
    public class PropNameMatchBaseConverter<TInterface, TModel> : IModelConverter
        where TInterface : class
        where TModel : class
    {
        private readonly IList<MatchedPropertyInfo> _matchedProperties;

        public PropNameMatchBaseConverter()
        {
            InterfaceType = typeof (TInterface);
            ModelType = typeof (TModel);
            if (!InterfaceType.IsInterface) throw new ArgumentException("Must be an interface type", "TInterface");
            if (!ModelType.IsClass) throw new ArgumentException("Must be a class type", "TModel");

            var interfaceProperties = TypeDescriptor.GetProperties(InterfaceType).OfType<PropertyDescriptor>().ToList();
            var modelProperties = TypeDescriptor.GetProperties(ModelType).OfType<PropertyDescriptor>().ToList();
            _matchedProperties = (from interfaceProp in interfaceProperties
                from modelProp in modelProperties
                where interfaceProp.Name == modelProp.Name
                select new MatchedPropertyInfo {InterfaceProperty = interfaceProp, ModelProperty = modelProp})
                .ToList();
        }

        public Type InterfaceType { get; private set; }
        public Type ModelType { get; private set; }

        public object ConvertToModel(object target)
        {
            return ConvertToModel((TInterface) target);
        }

        public virtual object ConvertFromModel(object model)
        {
            return ConvertFromModel((TModel) model);
        }

        protected virtual TModel ConvertToModel(TInterface target)
        {
            var model = Activator.CreateInstance<TModel>();
            foreach (var propertyInfo in _matchedProperties)
            {
                var propValue = propertyInfo.InterfaceProperty.GetValue(target);
                propertyInfo.ModelProperty.SetValue(model, propValue);
            }
            return model;
        }

        protected virtual TInterface ConvertFromModel(TModel model)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyInfo in _matchedProperties)
            {
                var propValue = propertyInfo.ModelProperty.GetValue(model);
                expando.Add(propertyInfo.ModelProperty.Name, propValue);
            }
            var target = expando.ActLike<TInterface>();
            return target;
        }
    }
}