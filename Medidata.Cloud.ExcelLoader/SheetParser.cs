using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using ImpromptuInterface;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader
{
    internal class SheetParser<T> : ISheetParser<T> where T : class
    {
        private readonly ICellTypeValueConverterFactory _converterFactory;
        private Worksheet _worksheet;

        public SheetParser(ICellTypeValueConverterFactory converterFactory)
        {
            if (converterFactory == null) throw new ArgumentNullException("converterFactory");
            _converterFactory = converterFactory;
        }

        public bool HasHeaderRow { get; set; }

        public IEnumerable<T> GetObjects()
        {
            string[] columnNames = null;
            if (typeof (T).ContainsDynamicFields())
            {
                var properties = typeof (T).GetPropertyDescriptors();
                var header = _worksheet.Descendants<Row>().First();
                var headerCells = header.Descendants<Cell>();
                columnNames = headerCells.Skip(properties.Count()).Select(c => c.InnerText).ToArray();
            }
            var rows = _worksheet.Descendants<Row>().Skip(HasHeaderRow ? 1 : 0);

            return rows.Select(e=>ParseFromRow(e, columnNames));
        }

        public void Load(Worksheet worksheet)
        {
            _worksheet = worksheet;
        }

        private T ParseFromRow(Row row, string[] dynamicColumnNames = null)
        {
            var properties = typeof (T).GetPropertyDescriptors();
            IDictionary<string, object> expando = new ExpandoObject();
            var cells = row.Elements<Cell>().ToList();
            var index = 0;
            foreach (var prop in properties)
            {
                var converter = _converterFactory.Produce(prop.PropertyType);
                var propValue = converter.GetCSharpValue(cells[index].InnerText);
                expando.Add(prop.Name, propValue);
                index ++;
            }
            
            //Logic for dynamic fields
            List<string> dynamicFields = new List<string>();
            if (dynamicColumnNames != null)
            {
                //TODO: Add logic to support more types
                expando.Add("DynamicFields", dynamicFields);
                expando.Add("DynamicColumnNames", dynamicColumnNames);
                for (int i = index; i < cells.Count; i++)
                {
                    dynamicFields.Add(cells[i].InnerText);
                }
            }

            T target = expando.ActLike();
            return target;
        }
    }
}