using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ImpromptuInterface;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader
{
    public abstract class SheetBuilder<T> : List<T>, ISheetBuilder where T : class
    {
        private readonly ICellTypeValueConverterFactory _converterFactory;

        protected SheetBuilder(ICellTypeValueConverterFactory converterFactory)
        {
            if (converterFactory == null) throw new ArgumentNullException("converterFactory");
            _converterFactory = converterFactory;
        }

        public bool HasHeaderRow { get; set; }
        public string SheetName { get; set; }
        public string[] ColumnNames { get; set; }

        public void AttachTo(SpreadsheetDocument doc)
        {
            var sheets = doc.WorkbookPart.Workbook.Sheets ?? doc.WorkbookPart.Workbook.AppendChild(new Sheets());

            var worksheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = CreateWorksheet();

            var sheetId = (uint) (1 + sheets.Count());
            var sheet = new Sheet
            {
                Id = doc.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = sheetId,
                Name = SheetName
            };

            // Use this attribute to retrieve the worksheet.
            var attribute = SpreadsheetAttributeHelper.CreateSheetNameAttribute(SheetName);
            sheet.SetAttribute(attribute);

            sheets.Append(sheet);
        }

        private Worksheet CreateWorksheet()
        {
            var sheetData = GetSheetData();
            var columns = GetColumns(sheetData);
            return columns != null && columns.Any() ? new Worksheet(columns, sheetData) : new Worksheet(sheetData);
        }

        private Columns GetColumns(SheetData sheetData)
        {
            var numberOfColumns = 0;
            if (sheetData.Descendants<Row>().Any())
            {
                numberOfColumns = sheetData.Descendants<Row>().First().Descendants<Cell>().Count();
            }
            var cs = new Columns();
            for (var i = 0; i < numberOfColumns; i++)
            {
                var c = new Column
                {
                    Min = (uint) (i + 1),
                    Max = (uint) (i + 1),
                    Width = 20D,
                    CustomWidth = true
                };
                cs.Append(c);
            }
            return cs;
        }

        protected virtual Cell CreateCell(T model, PropertyDescriptor property)
        {
            var cell = new Cell();
            if (model != null)
            {
                var propValue = GetPropertyValue(property, model);
                var converter = _converterFactory.Produce(property.PropertyType);
                var cellType = converter.CellType;
                var cellValue = converter.GetCellValue(propValue);
                cell.DataType = cellType;

                if (cellType == CellValues.InlineString)
                {
                    cell.InlineString = new InlineString {Text = new Text(cellValue)};
                }
                else
                {
                    cell.CellValue = new CellValue(cellValue);
                }
            }
            return cell;
        }

        protected Cell CreateDynamicCell(object field)
        {
            var cell = new Cell();
            var converter = _converterFactory.Produce(field.GetType());
            var cellType = converter.CellType;
            var cellValue = converter.GetCellValue(field);
            cell.DataType = cellType;
            if (cellType == CellValues.InlineString)
            {
                cell.InlineString = new InlineString { Text = new Text(cellValue) };
            }
            else
            {
                cell.CellValue = new CellValue(cellValue);
            }
            return cell;
        }

        //TODO: Find a way to change the catch-and-release logic, it's dirty and it might hurt the performance
        private object GetPropertyValue(PropertyDescriptor property, object target)
        {
            try
            {
                return property.GetValue(target);
            }
            catch
            {
                return null;
            }
        }

        private Row CreateRow(T model)
        {
            var row = new Row();
            var properties = model.GetType().GetPropertyDescriptors();
            IList dynamicFields;
            foreach (var prop in properties)
            {
                //Ignore Generic Collection, they are dynamic and should be added to the end of the row
                if(typeof(IList).IsAssignableFrom(prop.PropertyType))
                {
                    continue;
                }
                var cell = CreateCell(model, prop);
                row.AppendChild(cell);
            }
            if (!model.TryGetDynamicFields(out dynamicFields)) return row;
            foreach (var field in dynamicFields)
            {
                var cell = CreateDynamicCell(field);
                row.AppendChild(cell);
            }
            return row;
        }
        

        protected virtual Row CreateHeaderRow()
        {
            var row = new Row();
            foreach (var columnName in ColumnNames)
            {
                var cell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(columnName)
                };
                row.AppendChild(cell);
            }
            return row;
        }

        private SheetData GetSheetData()
        {
            var sheetData = new SheetData();
            if (HasHeaderRow)
            {
                var headerRow = CreateHeaderRow();
                sheetData.Append(headerRow);
            }

            var rows = this.Select(x => x ?? x.ActLike<T>()).Select(CreateRow);
            sheetData.Append(rows);

            return sheetData;
        }
    }
}