using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("TierFolders")]
    public class TierFolder : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("Form")]
        public string Form { get; set; }

        [ColumnHeaderName("FormOID")]
        public string FormOid { get; set; }
    }
}