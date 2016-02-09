using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Rave.Tsdv.Loader.ColumnResources;
using Medidata.Rave.Tsdv.Loader.Helpers;

namespace Medidata.Rave.Tsdv.Loader
{
    public class AutoCopyrightCoveredResourcedExcelBuilder : AutoCopyrightCoveredExcelBuilder
    {
        //Move "__Resources__" to config
        private const string resourceTabName = "__Resources__";
        private readonly List<ColumnResource> _resources;

        public AutoCopyrightCoveredResourcedExcelBuilder(IColumnResourceManager resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            _resources = resources.Resources;
        }

        protected override void PostBuildSheets(SpreadsheetDocument doc)
        {
            base.PostBuildSheets(doc);
            BuildResourceSheet(doc, resourceTabName);
        }


        private void BuildResourceSheet(SpreadsheetDocument doc, string tabName)
        {
            if (doc == null) throw new ArgumentNullException("doc");
            var sheets = doc.WorkbookPart.Workbook.Sheets ?? doc.WorkbookPart.Workbook.AppendChild(new Sheets());
            if (doc.SheetExist(tabName))
            {
                return;
            }


            var worksheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();

            worksheetPart.Worksheet = CreateWorksheet(doc, tabName);

            //Make sure it's the last tab
            var sheetId = 9999;
            var sheet = new Sheet
            {
                Id = doc.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = (uint) sheetId,
                Name = tabName
            };

            sheets.Append(sheet);
        }


        private Worksheet CreateWorksheet(SpreadsheetDocument doc, string tabName)
        {
            var sheetData = AddResourceColumns(doc, tabName);
            var worksheet = new Worksheet(sheetData);
            worksheet.AddMdsolNamespaceDeclaration();
            return worksheet;
        }


        private SheetData AddResourceColumns(SpreadsheetDocument doc, string tabName)
        {
            foreach (var resource in _resources)
            {
                resource.List = resource.List.OrderBy(o => o.Category).ThenBy(o => o.Value).ToList();
            }

            var sd = new SheetData();

            var headerRow = new Row();
            for (var i = 0; i < _resources.Count; i++)
            {
                var headerCell = new Cell
                {
                    CellValue = new CellValue(_resources[i].ResourceName),
                    DataType = new EnumValue<CellValues>(CellValues.String),
                    CellReference = (i + 1).ConvertToColumnName() + 1
                };
                headerRow.Append(headerCell);
            }
            sd.Append(headerRow);

            var resourceCount = _resources.Count;
            var maxOfResourceItems = _resources.Max(r => r.List.Count);

            var definedNameDict = new Dictionary<string, List<ResourceIndex>>();

            for (var i = 0; i < maxOfResourceItems; i++)
            {
                var contentRow = new Row();
                for (var j = 0; j < resourceCount; j++)
                {
                    if (i >= _resources[j].List.Count)
                    {
                        continue;
                    }
                    var str = _resources[j].List[i].Value;
                    var category = _resources[j].List[i].Category;
                    var columnName = (j + 1).ConvertToColumnName();
                    var contentCell = new Cell
                    {
                        CellValue = new CellValue(str),
                        //TODO: Currently only support string, will add more types if needed
                        DataType = new EnumValue<CellValues>(CellValues.String),
                        CellReference = columnName + (i + 2)
                    };
                    var key = category;

                    if (!definedNameDict.ContainsKey(key))
                    {
                        definedNameDict.Add(key,
                            new List<ResourceIndex> {new ResourceIndex {Column = columnName, Row = i + 2}});
                    }
                    else
                    {
                        definedNameDict[key].Add(new ResourceIndex {Column = columnName, Row = i + 2});
                    }
                    contentRow.Append(contentCell);
                }
                sd.Append(contentRow);
            }
            AddDefinedNames(doc, definedNameDict, tabName);
            return sd;
        }

        private void AddDefinedNames(SpreadsheetDocument doc, Dictionary<string, List<ResourceIndex>> definedNameDict,
            string tabName)
        {
            if (doc.WorkbookPart.Workbook.DefinedNames == null)
            {
                doc.WorkbookPart.Workbook.Append(new DefinedNames());
            }

            foreach (var name in definedNameDict)
            {
                var range = MakeRange(tabName, name.Value);
                doc.WorkbookPart.Workbook.DefinedNames.Append(new DefinedName {Name = name.Key, Text = range});
            }
        }

        private string MakeRange(string tabName, List<ResourceIndex> indices)
        {
            if (indices == null || !indices.Any()) return null;
            var min = indices.Min(o => o.Row);
            var max = indices.Max(o => o.Row);
            var columnName = indices.First().Column;
            return string.Format("{0}!${1}${2}:${3}${4}", tabName, columnName, min, columnName, max);
        }


        private class ResourceIndex
        {
            public string Column { get; set; }
            public int Row { get; set; }
        }
    }
}