using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.Tsdv.Loader.Extensions;

namespace Medidata.Cloud.Tsdv.Loader
{
    public class WorkbookBuilder : IWorkbookBuilder
    {
        private readonly IModelConverterFactory _modelConverterFactory;
        private readonly IExcelConverterFactory _excelConverterFactory;

        private readonly IDictionary<string, IWorksheetBuilder> _sheets =
            new Dictionary<string, IWorksheetBuilder>(StringComparer.OrdinalIgnoreCase);

        public WorkbookBuilder(IModelConverterFactory modelConverterFactory, IExcelConverterFactory excelConverterFactory)
        {
            if (modelConverterFactory == null) throw new ArgumentNullException("modelConverterFactory");
            if (excelConverterFactory == null) throw new ArgumentNullException("excelConverterFactory");
            _modelConverterFactory = modelConverterFactory;
            _excelConverterFactory = excelConverterFactory;
        }

        public ICollection<T> EnsureWorksheet<T>(string sheetName) where T : class
        {
            IWorksheetBuilder worksheetBuilder;
            if (!_sheets.TryGetValue(sheetName, out worksheetBuilder))
            {
                worksheetBuilder = new WorksheetBuilder<T>(_modelConverterFactory,_excelConverterFactory);
                _sheets.Add(sheetName, worksheetBuilder);
            }
            return ((ICollection<T>)worksheetBuilder);
        }

        public Workbook ToWorkbook(string workbookName, SpreadsheetDocument doc)
        {
            var workbookpart = doc.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            
            foreach (var sheetInfo in _sheets)
            {
                var sheetName = sheetInfo.Key;
                IWorksheetBuilder sheetBuilder = sheetInfo.Value;
                Worksheet sheetData = sheetBuilder.ToWorksheet(sheetName);
                var newWorksheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = sheetData;
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
                    Name = sheetName
                };
                sheets.Append(sheet);
                
            }
            workbookpart.Workbook.Save();
            return workbookpart.Workbook;
        }
    }
}