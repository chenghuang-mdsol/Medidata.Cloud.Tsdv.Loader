using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medidata.Cloud.Tsdv.Loader.Converters
{
    public class MiddleData
    {
        public IList<string> ColumnNames { get; set; }
        public IList<string> RowData { get; set; }

        public MiddleData(IList<string> columnNames, IList<string> rowData)
        {
            ColumnNames = columnNames;
            RowData = rowData;
        }
        public MiddleData() { }
    }
}
