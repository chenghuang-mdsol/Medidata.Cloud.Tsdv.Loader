using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierForms")]
    public class TierForm : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("FormOID")]
        public string FormOid { get; set; }

        [ColumnHeaderName("tsdv_Selected")]
        public bool Selected { get; set; }
    }
}