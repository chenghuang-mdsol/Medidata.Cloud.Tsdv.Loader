using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Medidata.Cloud.Tsdv.Loader.Converters
{
    public interface IConverter<T> : IConverter
    {
        new T ConvertBack(MiddleData data);
        MiddleData Convert(T obj);
    }

    public interface IConverter
    {
        object ConvertBack(MiddleData data);
        MiddleData Convert(object obj);
    }

    public class DefaultConverter: IConverter
    {
        public object Object { get; set; }

        
        public object ConvertBack(MiddleData data)
        {
            var columnNames = data.ColumnNames;
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
            IList<string> columnNames = new List<string>();
            IList<string> rowData = new List<string>();
            var pinfos = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pinfos)
            {
                columnNames.Add(pi.Name);
                rowData.Add(pi.GetValue(obj,null).ToString());
            }
            return new MiddleData(columnNames,rowData);
        }
    }
}
