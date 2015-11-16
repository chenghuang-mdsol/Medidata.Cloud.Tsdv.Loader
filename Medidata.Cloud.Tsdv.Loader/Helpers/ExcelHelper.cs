using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.Tsdv.Loader.Attributes;
using Medidata.Cloud.Tsdv.Loader.Converters;

namespace Medidata.Cloud.Tsdv.Loader.Helpers
{

    public enum NameMappingDirection
    {
        ToExcel,
        FromExcel
    }

    public class ExcelHelper
    {
        private Dictionary<string, string> GetSheetDisplayName(PropertyInfo[] props, NameMappingDirection direction)
        {
            Dictionary<string, string> nameMapping = new Dictionary<string, string>();
            foreach (PropertyInfo propertyInfo in props)
            {
                if (Attribute.IsDefined(propertyInfo, typeof (ExcelSheetAttribute)))
                {
                    ExcelSheetAttribute attr =
                        (ExcelSheetAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof (ExcelSheetAttribute));
                    if (attr.Excluded || !typeof (IList).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        continue;
                    }
                    if (direction == NameMappingDirection.FromExcel)
                    {
                        nameMapping[attr.SheetName] = propertyInfo.Name;
                    }
                    else
                    {
                        nameMapping[propertyInfo.Name] = attr.SheetName;
                    }
                }
                else
                {
                    nameMapping.Add(propertyInfo.Name,propertyInfo.Name);
                }
            }
            return nameMapping;
        }

        public SpreadsheetDocument ConvertToExcel(object obj, Stream stream)
        {
            SpreadsheetDocument doc = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            
            var workbookpart = doc.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            
            var props = obj.GetType().GetProperties();
            var nameMapping = GetSheetDisplayName(props,NameMappingDirection.ToExcel);
            Workbook workbook = new Workbook();
            workbook.AppendChild(new Sheets());
            foreach (PropertyInfo prop in props)
            {
                if (!nameMapping.ContainsKey(prop.Name))
                {
                    continue;
                }
                var list = (IList)prop.GetValue(obj, null);
                var converter = GetConverter(prop);
                SheetData sheetData = ConvertToWorkSheet(list,converter);
                var newWorksheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(sheetData);
                var sheets = doc.WorkbookPart.Workbook.AppendChild(new Sheets());

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Any())
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                var sheet = new Sheet()
                {
                    Id = doc.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = sheetId,
                    Name = nameMapping[prop.Name]
                };
                sheets.Append(sheet);
                workbookpart.Workbook.Save();
            }
            return doc;
        }

        public void ConvertToExcelAndSave(string fileName, object obj)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (SpreadsheetDocument doc = ConvertToExcel(obj,fs))
                {
                    
                    fs.Flush();
                }
            }
            
        }
        public SheetData ConvertToWorkSheet(IList objects, IConverter converter)
        {
            
            if (objects == null || objects.Count == 0)
            {
                return null;
            }
            SheetData sheetData = new SheetData();
            
            foreach (object obj in objects)
            {
                MiddleData data = converter.Convert(obj);
                if (!sheetData.Descendants<Row>().Any())
                {
                    Row row = new Row();
                    foreach (var c in data.ColumnNames)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(c);
                        row.AppendChild(cell);
                    }
                    sheetData.Append(row);
                }
                Row dataRow = new Row();
                foreach (var r in data.RowData)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(r);
                    dataRow.AppendChild(cell);
                }
                sheetData.Append(dataRow);
            }

            return sheetData;
        }

        private IConverter GetConverter(PropertyInfo pi)
        {
            
            IConverter converter;
            if (Attribute.IsDefined(pi, typeof(ExcelSheetAttribute),true))
            {
                ExcelSheetAttribute attr =
                    (ExcelSheetAttribute)Attribute.GetCustomAttribute(pi, typeof(ExcelSheetAttribute));
                Type converterType = attr.ConverterType;
                if (converterType != null)
                {
                    converter = (IConverter)Activator.CreateInstance(converterType);
                }
                else
                {
                    converter = new DefaultConverter();
                }
            }
            else
            {
                converter = new DefaultConverter();
            }
            return converter;
        }
            


        public void CreateSpreadsheetWorkbook(string filepath, Workbook workbook)
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook))
            {
                var workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = workbook;
                workbookpart.Workbook.Save();
            }


        }

        public T GetObjectsFromSpreadsheetWorkbook<T>(SpreadsheetDocument doc) where T : new()
        {
            T obj= new T();
            var props = typeof (T).GetProperties();
            var nameMapping = GetSheetDisplayName(props,NameMappingDirection.FromExcel);
            WorkbookPart workBookPart = doc.WorkbookPart;
            foreach (Sheet sheet in workBookPart.Workbook.Descendants<Sheet>())
            {
                
                if (nameMapping.ContainsKey(sheet.Name))
                {
                    var propertyName = nameMapping[sheet.Name];
                    var sheetID = sheet.Id;
                    WorksheetPart worksheetPart = doc.WorkbookPart.GetPartById(sheetID) as WorksheetPart;
                    var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    var propertyInfo = obj.GetType().GetProperty(propertyName);
                    propertyInfo.SetValue(obj,GetObjetsFromSheetData(obj,sheetData,propertyInfo),null);
                }
            }
            return obj;

        }

        public T ConvertFromExcel<T>(string filePath) where T: new()
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, isEditable: false))
            {
                return GetObjectsFromSpreadsheetWorkbook<T>(doc);
            }
        }


        private string GetColumnAddress(string cellReference)
        {
            //Create a regular expression to get column address letters.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        } 
        public IList GetObjetsFromSheetData(object parentObject, SheetData sheetData, PropertyInfo pi)
        {
            
            if (!typeof(IList).IsAssignableFrom(pi.PropertyType))
            {
                return null;
            }
            var innerType = pi.PropertyType.GetGenericArguments().First();
            var baseType = typeof (List<>);
            var genericType = baseType.MakeGenericType(innerType);
            var resultList = (IList)Activator.CreateInstance(genericType);

            IConverter converter = GetConverter(pi);
        
            var columnNames = new List<string>();
            //var columnLetters = new List<string>();
            foreach (Cell cell in sheetData.Descendants<Row>().First().Descendants<Cell>())
            {
                //Get custom column names.
                //Remove spaces, symbols (except underscore), and make lower cases and for all values in columnNames list.                    
                //columnNames.Add(Regex.Replace(cell.CellValue.Text, @"[^A-Za-z0-9_]", "").ToLower());
                columnNames.Add(cell.CellValue.Text.ToLower());
            }


            var rowData = new List<string>();

            MiddleData data = new MiddleData(columnNames,rowData);
            foreach (var row in sheetData.Descendants<Row>().Skip(1))
            {
                rowData.Clear();

                foreach (var cell in row.Descendants<Cell>())
                {
                    rowData.Add(cell.CellValue.Text);
                }
                resultList.Add(converter.ConvertBack(data));
            }
            return resultList;
        }

        private IEnumerable<Cell> GetCellsForRow(Row row, List<string> columnLetters)
        {
            int workIdx = 0;
            foreach (var cell in row.Descendants<Cell>())
            {
                //Get letter part of cell address.
                var cellLetter = GetColumnAddress(cell.CellReference);

                //Get column index of the matched cell.  
                int currentActualIdx = columnLetters.IndexOf(cellLetter);

                //Add empty cell if work index smaller than actual index.
                for (; workIdx < currentActualIdx; workIdx++)
                {
                    var emptyCell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptyCell;
                }

                //Return cell with data from Excel row.
                yield return cell;
                workIdx++;

                //Check if it's ending cell but there still is any unmatched columnLetters item.   
                if (cell == row.LastChild)
                {
                    //Append empty cells to enumerable. 
                    for (; workIdx < columnLetters.Count(); workIdx++)
                    {
                        var emptyCell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                        yield return emptyCell;
                    }
                }
            }
        }


        private IEnumerable<Row> GetUsedRows(SheetData sheetData)
        {
            bool hasValue;
            //Iterate all rows except the first one.
            foreach (var row in sheetData.Descendants<Row>().Skip(1))
            {
                hasValue = false;
                foreach (var cell in row.Descendants<Cell>())
                {
                    //Find at least one cell with value for a row
                    if (!string.IsNullOrEmpty(cell.CellValue.Text))
                    {
                        hasValue = true;
                        break;
                    }
                }
                if (hasValue)
                {
                    //Return the row and keep iteration state.
                    yield return row;
                }
            }
        }


        private string GetColumnName(string cellReference)
        {
            if (cellReference == null)
                return null;

            return Regex.Replace(cellReference, "[0-9]", "");
        }


        
            
            
        
    }
}