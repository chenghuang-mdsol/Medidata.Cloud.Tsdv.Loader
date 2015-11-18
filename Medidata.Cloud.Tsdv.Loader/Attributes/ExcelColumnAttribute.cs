using System;

namespace Medidata.Cloud.Tsdv.Loader.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelColumnAttribute : Attribute
    {
        
        public string LocalizationKey { get; set; }
    }
}