using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Medidata.Cloud.Tsdv.Loader.Builders
{
    public class CoverWorksheetBuilder : List<object>, IWorksheetBuilder
    {
        private static readonly object CoverSheetLock = new object();
        private WorksheetPart _coverWorksheetPart;
        public string[] ColumnNames { get; set; }

        public void AppendWorksheet(SpreadsheetDocument doc, bool hasHeaderRow, string sheetName)
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
                Name = sheetName,
                Id = doc.WorkbookPart.GetIdOfPart(clonedSheetPart),
                SheetId = (uint)sheets.ChildElements.Count + 1
            };
            sheets.Append(copiedSheet);
            doc.WorkbookPart.Workbook.Save();
        }

        public WorksheetPart GetCoverWorksheetPart()
        {
            if (_coverWorksheetPart != null) return _coverWorksheetPart;
            lock (CoverSheetLock)
            {
                if (_coverWorksheetPart != null) return _coverWorksheetPart;
                var sheetBytes = Resource.CoverSheet;
                using (var ms = new MemoryStream())
                {
                    ms.Write(sheetBytes, 0, sheetBytes.Length);
                    var ss = SpreadsheetDocument.Open(ms, false);
                    var coverSheetId = ss.WorkbookPart.Workbook.Descendants<Sheet>().First().Id;
                    _coverWorksheetPart = (WorksheetPart)ss.WorkbookPart.GetPartById(coverSheetId);
                }
            }

            return _coverWorksheetPart;
        }
    }
}