using System;

namespace Medidata.Cloud.ExcelLoader
{
    public interface IColumnDefinition
    {
        Type PropertyType { get; set; }
        string PropertyName { get; set; }
        string Header { get; set; }
    }
}