using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader
{
    public class CoveredExcelBuilder : ExcelBuilder
    {
        protected override SpreadsheetDocument CreateDocument(Stream outStream)
        {
            var sheetBytes = Resource.CoverSheet;
            outStream.Write(sheetBytes, 0, sheetBytes.Length);
            return SpreadsheetDocument.Open(outStream, true);
        }
    }
}