using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
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
            AssemblyCopyRight = "No copyright information found. <AssemblyCopyrightAttribute> not defined in the calling assembly.";
            var asm = GetAppEntryAssembly();
            if (asm == null) return;
            var asmAttribute = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true)
                                  .OfType<AssemblyCopyrightAttribute>()
                                  .FirstOrDefault();
            if (asmAttribute == null) return;
            AssemblyCopyRight = asmAttribute.Copyright;
        }

        private static Assembly GetAppEntryAssembly()
        {
            var httpCurrent = HttpContext.Current;
            if (httpCurrent == null || httpCurrent.Handler == null)
            {
                // Not a web application
                return Assembly.GetEntryAssembly();
            }
            // Web application
            var memberInfo = httpCurrent.Handler.GetType().BaseType;
            return memberInfo != null ? memberInfo.Assembly : null;
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