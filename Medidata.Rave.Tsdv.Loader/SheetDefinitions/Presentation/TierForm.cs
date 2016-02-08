using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("TierForms")]
    public class TierForm : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("Forms", "Forms.FormOID")]
        public string Forms { get; set; }

        [ColumnInfo("FormOID", "FormOIDSource")]
        public string FormOid { get; set; }

        [ColumnInfo("tsdv_FieldsSelected")]
        public bool FieldsSelected { get; set; }

        [ColumnInfo("tsdv_FoldersSelected")]
        public bool FoldersSelected { get; set; }
    }
}