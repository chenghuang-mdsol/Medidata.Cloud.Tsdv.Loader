using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader
{
    public interface ISheetDefinition
    {
        string Name { get; set; }
        int HeaderRowCount { get; set; }
        IEnumerable<IColumnDefinition> ColumnDefinitions { get; }
        ISheetDefinition AddColumn(IColumnDefinition columnDefinition);
    }
}