using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Medidata.Rave.Tsdv.Loader.Helpers
{
    public static class SpreadsheetExtensions
    {
        public static bool SheetExist(this SpreadsheetDocument doc, string sheetName)
        {

            if (doc == null) throw new ArgumentNullException("doc");
            if (doc.WorkbookPart == null) throw new ArgumentNullException("WorkbookPart");
            var wbPart = doc.WorkbookPart;
            Sheet sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            return sheet != null;
        }


        public static bool TryGetFirstSheetByName(this SpreadsheetDocument doc, string sheetName, out Sheet result)
        {

            if (doc == null) throw new ArgumentNullException("doc");
            if (doc.WorkbookPart == null) throw new ArgumentNullException("WorkbookPart");
            var wbPart = doc.WorkbookPart;
            Sheet sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet == null)
            {
                result = null;
                return false;
            }
            result = sheet;
            return true;
        }


    }
}
