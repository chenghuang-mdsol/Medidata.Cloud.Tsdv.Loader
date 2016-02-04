using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader
{
    public class SheetParser : ISheetParser
    {
        private readonly ICellTypeValueConverterManager _converterManager;

        public SheetParser(ICellTypeValueConverterManager converterManager)
        {
            if (converterManager == null) throw new ArgumentNullException("converterManager");
            _converterManager = converterManager;
        }

        public IEnumerable<ExpandoObject> GetObjects(Worksheet worksheet, ISheetDefinition sheetDefinition)
        {
            if (worksheet == null) throw new ArgumentNullException("worksheet");
            if (sheetDefinition == null) throw new ArgumentNullException("sheetDefinition");
            var rows = worksheet.Descendants<Row>().Skip(sheetDefinition.HeaderRowCount);
            return rows.Select(x => ParseFromRow(x, sheetDefinition));
        }

        private ExpandoObject ParseFromRow(Row row, ISheetDefinition sheetDefinition)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var cells = row.Elements<Cell>().ToList();
            var colDefs = sheetDefinition.ColumnDefinitions.ToList();
            var count = Math.Max(cells.Count, colDefs.Count);
            for (var index = 0; index < count; index++)
            {
                var colDef = index < colDefs.Count ? colDefs[index] : null;
                var cell = index < cells.Count ? cells[index] : null;
                string propName;
                object propValue;
                if (cell != null)
                {
                    propName = cell.GetMdsolAttribute("propertyName");
                    propValue = _converterManager.GetCSharpValue(cell);
                }
                else if (colDef != null)
                {
                    propName = colDef.PropertyName;
                    propValue = null;
                }
                else
                {
                    throw new NotSupportedException("Cell and CellDefinition are both null");
                }

                expando.Add(propName, propValue);
            }
            return (ExpandoObject) expando;
        }
    }
}