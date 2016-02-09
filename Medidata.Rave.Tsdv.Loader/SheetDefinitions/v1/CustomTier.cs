using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [ExcludeFromCodeCoverage]
    [SheetName("CustomTiers")]
    public class CustomTier : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("tsdv_TierDescription")]
        public string TierDescription { get; set; }
    }
}