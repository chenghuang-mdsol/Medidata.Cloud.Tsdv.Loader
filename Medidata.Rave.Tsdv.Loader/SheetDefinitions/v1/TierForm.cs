using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [ExcludeFromCodeCoverage]
    [SheetName("TierForms")]
    public class TierForm : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("FormOID", "FormOIDSource")]
        public string FormOid { get; set; }

        [ColumnInfo("tsdv_Selected")]
        public bool Selected { get; set; }
    }
}