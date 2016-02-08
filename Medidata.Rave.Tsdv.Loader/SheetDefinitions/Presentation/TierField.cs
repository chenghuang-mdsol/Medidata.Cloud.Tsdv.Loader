using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("TierFields")]
    public class TierField : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("FormOID", "FormOIDSource")]
        public string FormOid { get; set; }

        [ColumnInfo("Fields", "Fields.FormOID")]
        public string Fields { get; set; }

        [ColumnInfo("IsLog")]
        public bool IsLog { get; set; }

        [ColumnInfo("tsdv_ControlType")]
        public string ControlType { get; set; }

        [ColumnInfo("tsdv_RequiresVerification")]
        public bool RequiresVerification { get; set; }
    }
}