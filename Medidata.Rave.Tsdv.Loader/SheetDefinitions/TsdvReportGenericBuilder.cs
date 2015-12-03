using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions
{
    internal class TsdvReportGenericBuilder : ExcelBuilder
    {
        private readonly ILocalization _localization;

        public TsdvReportGenericBuilder(ICellStyleProvider cellStyleProvider, ISheetBuilderFactory sheetBuilderFactory,
            ILocalization localization)
            : base(cellStyleProvider, sheetBuilderFactory)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        protected override string[] GetColumnNames<T>(string[] columnNames)
        {
            return columnNames == null
                ? base.GetColumnNames<T>(null)
                : columnNames.Select(x => _localization.GetLocalString(x)).ToArray();
        }

        protected override SpreadsheetDocument CreateDocument(Stream outStream)
        {
            var sheetBytes = Resource.CoverSheet;
            outStream.Write(sheetBytes, 0, sheetBytes.Length);
            var ss = SpreadsheetDocument.Open(outStream, true);
            return ss;
        }
    }
}