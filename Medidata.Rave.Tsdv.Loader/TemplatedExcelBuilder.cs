using System.IO;
using DocumentFormat.OpenXml.Packaging;
using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TemplatedExcelBuilder : ExcelBuilder
    {
        protected override SpreadsheetDocument CreateDocument(Stream outStream)
        {
            var sheetBytes = Resource.CoverSheet;
            outStream.Write(sheetBytes, 0, sheetBytes.Length);
            var ss = SpreadsheetDocument.Open(outStream, true);
            return ss;
        }
    }
}