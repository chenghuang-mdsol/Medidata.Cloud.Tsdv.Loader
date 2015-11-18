using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace Medidata.Cloud.Tsdv.Loader.Converters
{
    public class ColumnName
    {
        public string Name { get; set; }
        public string LocalizationKey { get; set; }
        public string RealName { get; set; }
        public ColumnName(string name, string realName, string localizationKey)
        {
            Name = name;
            RealName = realName;
            LocalizationKey = localizationKey;
        }
    }

    public class MiddleData
    {
        public IList<ColumnName> ColumnNames { get; set; }
        public IList<string> RowData { get; set; }
        public MiddleData(IList<ColumnName> columnNames, IList<string> rowData, string locale = null)
        {
            ColumnNames = columnNames;
            RowData = rowData;
        }
        public MiddleData() { }
    }
}
