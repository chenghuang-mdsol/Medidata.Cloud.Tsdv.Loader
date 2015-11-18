using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Medidata.Cloud.Tsdv.Loader.Converters
{
    public class DefaultConverter: IConverter
    {
        public object Object { get; set; }

        
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