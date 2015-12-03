using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions
{
    internal class TsdvReportGenericParser : ExcelParser
    {
        public TsdvReportGenericParser(ICellTypeValueConverterFactory converterFactory) : base(converterFactory)
        {
        }

        public string GetDocumentVersion()
        {
            return null;
        }
    }
}