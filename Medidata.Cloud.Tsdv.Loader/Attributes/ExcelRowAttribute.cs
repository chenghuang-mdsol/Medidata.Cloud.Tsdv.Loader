using System;

namespace Medidata.Cloud.Tsdv.Loader.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelRowAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public bool Excluded { get; set; }
        public string DefaultValue { get; set; }
    }



}