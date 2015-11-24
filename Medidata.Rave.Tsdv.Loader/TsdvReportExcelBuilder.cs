using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using DocumentFormat.OpenXml;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvReportExcelBuilder : ExcelBuilder
    {
        private readonly ILocalization _localization;

        public TsdvReportExcelBuilder(ILocalization localization)
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

        protected override SpreadsheetDocument CreateDocument(Stream outStream)
        {
            //return;
            var sheetBytes = Resource.CoverSheet;
            outStream.Write(sheetBytes, 0, sheetBytes.Length);
            SpreadsheetDocument ss = SpreadsheetDocument.Open(outStream, true);
            return ss;
            
        }
        private WorksheetPart GetCoverWorksheetPart()
        {
            var sheetBytes = Resource.CoverSheet;
            using (var ms = new MemoryStream())
            {
                ms.Write(sheetBytes, 0, sheetBytes.Length);
                using (SpreadsheetDocument ss = SpreadsheetDocument.Open(ms,true))
                {
                    var coverSheetId = ss.WorkbookPart.Workbook.Descendants<Sheet>().First().Id;
                    return (WorksheetPart)ss.WorkbookPart.GetPartById(coverSheetId);
                }
            }
        }
    }
}