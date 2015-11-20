using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Medidata.Cloud.Tsdv.Loader.Converters;
using Medidata.Interfaces.Localization;

namespace Medidata.Cloud.Tsdv.Loader.ExcelConverters
{
    public class DefaultConverter: IExcelConverter
    {
        public object Object { get; set; }


        public Type InterfaceType { get; set; }
        public ILocalization Localization { get; set; }

        public object ConvertBack(MiddleData data)
        {
            var columnNames = data.ColumnNames.Select(o=>o.Name).ToList();
            var rowData = data.RowData;
            var propDict = Object.GetType().GetProperties().ToDictionary(p => p.Name.ToLower());
            foreach (var c in columnNames)
            {
                if (propDict.ContainsKey(c.ToLower()))
                {
                    propDict[c.ToLower()].SetValue(rowData[columnNames.IndexFor(c)], Object, null);
                }
            }
            return Object;
        }

        public MiddleData Convert(object obj)
        {
            IList<ColumnName> columnNames = new List<ColumnName>();
            IList<string> rowData = new List<string>();
            var pinfos = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pinfos)
            {
                columnNames.Add(new ColumnName(pi.Name,pi.Name, pi.Name));
                rowData.Add(pi.GetValue(obj,null).ToString());
            }
            return new MiddleData(columnNames,rowData);
        }
    }
}