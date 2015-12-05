using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader
{
    public class ExcelBuilder : IExcelBuilder
    {
        private readonly ISheetBuilderFactory _sheetBuilderFactory;
        private readonly IList<ISheetBuilder> _sheetBuilders = new List<ISheetBuilder>();
        private readonly ICellStyleProvider _styleProvider;

        public ExcelBuilder(ICellStyleProvider styleProvider, ISheetBuilderFactory sheetBuilderFactory)
        {
            if (styleProvider == null) throw new ArgumentNullException("styleProvider");
            if (sheetBuilderFactory == null) throw new ArgumentNullException("sheetBuilderFactory");
            _styleProvider = styleProvider;
            _sheetBuilderFactory = sheetBuilderFactory;
        }

        public virtual IList<T> AddSheet<T>(string sheetName, string[] columnNames) where T : class
        {
            return AddSheet<T>(sheetName, columnNames != null, columnNames);
        }

        public virtual IList<T> AddSheet<T>(string sheetName, bool hasHeaderRow = true) where T : class
        {
            return AddSheet<T>(sheetName, hasHeaderRow, null);
        }

        public virtual IList<T> GetSheet<T>(string sheetName) where T : class
        {
            return (IList<T>) _sheetBuilders.First(x => x.SheetName != sheetName);
        }

        public void Save(Stream outStream)
        {
            using (var doc = CreateDocument(outStream))
            {
                WorkbookPart workbookPart;
                if (doc.WorkbookPart == null)
                {
                    workbookPart = doc.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                }
                else
                {
                    workbookPart = doc.WorkbookPart;
                }

                _styleProvider.AttachTo(doc);

                foreach (var sheet in _sheetBuilders)
                {
                    sheet.AttachTo(doc);
                }

                workbookPart.Workbook.Save();
            }
        }

        private IList<T> AddSheet<T>(string sheetName, bool hasHeaderRow, string[] columnNames, IDynamicFields obj = null)
            where T : class
        {
            if (_sheetBuilders.Any(x => x.SheetName == sheetName))
                throw new ArgumentException("Duplicate sheet name '" + sheetName + "'", "sheetName");

            var worksheetBuilder = _sheetBuilderFactory.Create<T>();
            worksheetBuilder.SheetName = sheetName;
            worksheetBuilder.HasHeaderRow = hasHeaderRow;
            worksheetBuilder.ColumnNames = GetColumnNames<T>(columnNames);
            _sheetBuilders.Add(worksheetBuilder);

            return (IList<T>) worksheetBuilder;
        }

        public void GenerateDynamicColumnNames()
        {
            foreach (ISheetBuilder builder in _sheetBuilders)
            {
                if (builder.Count > 0 && builder[0] is IDynamicFields)
                {
                    AddDynamicColumnNames(builder, ((IDynamicFields)builder[0]).DynamicColumnNames);
                }
            }
        }

        private void AddDynamicColumnNames (ISheetBuilder builder, string[] columnNames)
        {
            var list = new List<string>();
            list.AddRange(builder.ColumnNames);
            list.AddRange(columnNames.ToList());
            builder.ColumnNames = list.ToArray();
        }

        protected virtual string[] GetColumnNames<T>(string[] columnNames)
        {
            return columnNames ?? typeof (T).GetPropertyDescriptors().Select(p => p.Name).ToArray();
        }

        protected virtual SpreadsheetDocument CreateDocument(Stream outStream)
        {
            return SpreadsheetDocument.Create(outStream, SpreadsheetDocumentType.Workbook);
        }
    }
}