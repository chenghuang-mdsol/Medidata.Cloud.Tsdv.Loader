using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using DocumentFormat.OpenXml;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions
{
    internal class TsdvReportGenericBuilder : ExcelBuilder
    {
        private readonly ILocalization _localization;

        public TsdvReportGenericBuilder(ICellTypeValueConverterFactory converterFactory, ILocalization localization)
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

        protected override SpreadsheetDocument CreateDocument(Stream outStream)
        {
            //return;
            var sheetBytes = Resource.CoverSheet;
            outStream.Write(sheetBytes, 0, sheetBytes.Length);
            SpreadsheetDocument ss = SpreadsheetDocument.Open(outStream, true);
            return ss;
            
        }
    }
}