using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.CellTypeConverters;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvReportExcelBuilder : ExcelBuilder
    {
        private readonly ILocalization _localization;

        public TsdvReportExcelBuilder(ICellTypeValueConverterFactory converterFactory, ILocalization localization)
            : base(converterFactory)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        protected override string[] GetPropertyNames<T>(string[] columnNames)
        {
            return columnNames == null
                ? base.GetPropertyNames<T>(null)
                : columnNames.Select(x => _localization.GetLocalString(x)).ToArray();
        }

        protected override void AddCoverSheet(SpreadsheetDocument doc)
        {
            return;

            var coverWorkbookPart = GetCoverWorksheetPart();

            var tempSheet = SpreadsheetDocument.Create(new MemoryStream(), doc.DocumentType);
            var tempWorkbookPart = tempSheet.AddWorkbookPart();
            var tempWorksheetPart = tempWorkbookPart.AddPart(coverWorkbookPart);
            var clonedSheetPart = doc.WorkbookPart.AddPart(tempWorksheetPart);

            var sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var copiedSheet = new Sheet
            {
                Name = _localization.GetLocalString("Cover"),
                Id = doc.WorkbookPart.GetIdOfPart(clonedSheetPart),
                SheetId = (uint) sheets.ChildElements.Count + 1
            };
            sheets.Append(copiedSheet);
        }

        private WorksheetPart GetCoverWorksheetPart()
        {
            var sheetBytes = Resource.CoverSheet;
            using (var ms = new MemoryStream())
            {
                ms.Write(sheetBytes, 0, sheetBytes.Length);
                var ss = SpreadsheetDocument.Open(ms, false);
                var coverSheetId = ss.WorkbookPart.Workbook.Descendants<Sheet>().First().Id;
                return (WorksheetPart) ss.WorkbookPart.GetPartById(coverSheetId);
            }
        }
    }
}