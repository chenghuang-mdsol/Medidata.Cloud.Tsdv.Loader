﻿using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace Medidata.Cloud.Tsdv.Loader.ExcelConverters
{
    public class ColumnName
    {
        public string Name { get; set; }
        public string LocalizationKey { get; set; }
        public string PropertyName { get; set; }
        public ColumnName(string name, string propertyName, string localizationKey)
        {
            Name = name;
            PropertyName = propertyName;
            LocalizationKey = localizationKey;
        }
    }

    public class MiddleData
    {
        public IList<ColumnName> ColumnNames { get; set; }
        public IList<string> RowData { get; set; }
        public MiddleData(IList<ColumnName> columnNames, IList<string> rowData)
        {
            ColumnNames = columnNames;
            RowData = rowData;
        }
        public MiddleData() { }
    }
}
