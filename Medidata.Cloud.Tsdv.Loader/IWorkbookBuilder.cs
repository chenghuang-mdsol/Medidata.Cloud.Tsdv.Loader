using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Medidata.Cloud.Tsdv.Loader
{
    public interface IWorkbookBuilder
    {
        IWorksheetBuilder<T> EnsureWorksheet<T>(string sheetName) where T : class;
        Workbook ToWorkbook(string workbookName, SpreadsheetDocument spreadsheet);
    }
}