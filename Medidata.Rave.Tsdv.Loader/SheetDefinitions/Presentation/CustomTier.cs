using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("CustomTiers")]
    public class CustomTier : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("tsdv_TierDescription")]
        public string TierDescription { get; set; }

        [ColumnInfo("tsdv_LinkedToProdStudy")]
        public bool LinkedToProdStudy { get; set; }
    }
}