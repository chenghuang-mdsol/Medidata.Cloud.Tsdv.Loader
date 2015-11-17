﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.Tsdv.Loader.Attributes;
using Medidata.Cloud.Tsdv.Loader.Converters;

namespace Medidata.Cloud.Tsdv.Loader.Helpers
{
    public class ExcelHelper
    {
        public SpreadsheetDocument ConvertToExcel(object obj, Stream stream)
        {
            var doc = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            var workbookpart = doc.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            var props = obj.GetType().GetProperties();

            var workbook = new Workbook();
            workbook.AppendChild(new Sheets());
            foreach (var prop in props)
            {
                var list = (IList) prop.GetValue(obj, null);
                var converter = GetConverter(prop);
                var sheetData = ConvertToWorkSheet(list, converter);
                var newWorksheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(sheetData);
                var sheets = doc.WorkbookPart.Workbook.AppendChild(new Sheets());

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Any())
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                var sheet = new Sheet
                {
                    Id = doc.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = sheetId,
                    Name = prop.Name
                };
                sheets.Append(sheet);
                workbookpart.Workbook.Save();
            }
            return doc;
        }

        public void ConvertToExcelAndSave(string fileName, object obj)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                using (ConvertToExcel(obj, fs))
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
            var sheetData = new SheetData();
            foreach (var obj in objects)
            {
                var data = converter.Convert(obj);
                if (!sheetData.Descendants<Row>().Any())
                {
                    var row = new Row();
                    foreach (var c in data.ColumnNames)
                    {
                        var cell = new Cell();
                        //TODO: Add Localization Logic
                        cell.SetAttribute(new OpenXmlAttribute("RealName","mdsol",c));;
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(c);
                        row.AppendChild(cell);
                    }
                    sheetData.Append(row);
                }
                var dataRow = new Row();
                foreach (var r in data.RowData)
                {
                    var cell = new Cell();
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
            if (Attribute.IsDefined(pi, typeof (ExcelSheetAttribute), true))
            {
                var attr =
                    (ExcelSheetAttribute) Attribute.GetCustomAttribute(pi, typeof (ExcelSheetAttribute));
                var converterType = attr.ConverterType;
                if (converterType != null)
                {
                    converter = (IConverter) Activator.CreateInstance(converterType);
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
            var obj = new T();
            var workBookPart = doc.WorkbookPart;
            foreach (var sheet in workBookPart.Workbook.Descendants<Sheet>())
            {
                var propertyName = sheet.Name;
                var sheetID = sheet.Id;
                var worksheetPart = doc.WorkbookPart.GetPartById(sheetID) as WorksheetPart;
                var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(obj, GetObjetsFromSheetData(obj, sheetData, propertyInfo), null);
            }
            return obj;
        }

        public T ConvertFromExcel<T>(string filePath) where T : new()
        {
            using (var doc = SpreadsheetDocument.Open(filePath, false))
            {
                return GetObjectsFromSpreadsheetWorkbook<T>(doc);
            }
        }

        public IList GetObjetsFromSheetData(object parentObject, SheetData sheetData, PropertyInfo pi)
        {
            if (!typeof (IList).IsAssignableFrom(pi.PropertyType))
            {
                return null;
            }
            var innerType = pi.PropertyType.GetGenericArguments().First();
            var baseType = typeof (List<>);
            var genericType = baseType.MakeGenericType(innerType);
            var resultList = (IList) Activator.CreateInstance(genericType);

            var converter = GetConverter(pi);

            var columnNames =
                sheetData.Descendants<Row>()
                    .First()
                    .Descendants<Cell>()
                    .Select(cell => cell.CellValue.Text.ToLower())
                    .ToList();
            var realColumnNames =
                sheetData.Descendants<Row>()
                    .First()
                    .Descendants<Cell>()
                    .Select(cell => cell.GetAttributes().Any(a=>a.LocalName=="RealName")? cell.GetAttribute("RealName","mdsol").Value.ToLower(): cell.CellValue.Text.ToLower() )
                    .ToList();

            var rowData = new List<string>();

            var data = new MiddleData(columnNames, rowData, realColumnNames);
            foreach (var row in sheetData.Descendants<Row>().Skip(1))
            {
                rowData.Clear();
                rowData.AddRange(row.Descendants<Cell>().Select(cell => cell.CellValue.Text));
                resultList.Add(converter.ConvertBack(data));
            }
            return resultList;
        }
    }
}