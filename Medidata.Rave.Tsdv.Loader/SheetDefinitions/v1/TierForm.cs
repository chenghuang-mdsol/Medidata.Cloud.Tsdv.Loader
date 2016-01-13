using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierForms")]
    public class TierForm : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("Forms")]
        public string Forms { get; set; }

        [ColumnHeaderName("FormOID")]
        public string FormOid { get; set; }

        [ColumnHeaderName("tsdv_FieldsSelected")]
        public bool FieldsSelected { get; set; }

        [ColumnHeaderName("tsdv_FoldersSelected")]
        public bool FoldersSelected { get; set; }
    }
}