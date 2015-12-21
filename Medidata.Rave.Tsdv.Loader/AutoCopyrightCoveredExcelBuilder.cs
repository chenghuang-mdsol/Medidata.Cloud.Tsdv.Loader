using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Medidata.Rave.Tsdv.Loader
{
    public class AutoCopyrightCoveredExcelBuilder : CoveredExcelBuilder
    {
        private static readonly string AssemblyCopyRight;
        private static readonly Regex MatchingPattern;

        static AutoCopyrightCoveredExcelBuilder()
        {
            MatchingPattern = new Regex(@"Copyright \d+ Medidata Solutions");

            var asmAttribute = Assembly.GetEntryAssembly()
                                       .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true)
                                       .OfType<AssemblyCopyrightAttribute>()
                                       .FirstOrDefault();
            AssemblyCopyRight = asmAttribute == null
                ? "No copyright information found. <AssemblyCopyrightAttribute> not defined in the calling assembly."
                : asmAttribute.Copyright;
        }

        protected override SpreadsheetDocument CreateDocument(Stream outStream)
        {
            var doc = base.CreateDocument(outStream);

            var sharedStringTable = doc.WorkbookPart.SharedStringTablePart.SharedStringTable;
            var strings = sharedStringTable.Elements<SharedStringItem>()
                                           .Where(x => MatchingPattern.IsMatch(x.InnerText));

            foreach (var s in strings)
            {
                s.Text = new Text(AssemblyCopyRight);
            }

            sharedStringTable.Save();

            return doc;
        }
    }
}