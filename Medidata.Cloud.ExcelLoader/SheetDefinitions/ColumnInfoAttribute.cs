using System;

namespace Medidata.Cloud.ExcelLoader.SheetDefinitions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnInfoAttribute : Attribute
    {
        public ColumnInfoAttribute(string header, string source = null)
        {
            Header = header;
            Source = source;
        }

        public string Header { get; set; }

        //For direct source [Source = {SourceName}]
        //For indirect source [Source = {Header}.{DependentHeader}]
        public string Source { get; set; }

    }



}