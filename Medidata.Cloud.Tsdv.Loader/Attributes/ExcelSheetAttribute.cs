using System;
using System.Collections.Generic;
using Medidata.Cloud.Tsdv.Loader.Converters;

namespace Medidata.Cloud.Tsdv.Loader.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelSheetAttribute :Attribute
    {
        public string SheetName { get; set; }
        public bool Excluded { get; set; }
        public Type ConverterType { get; set; }
    }
}