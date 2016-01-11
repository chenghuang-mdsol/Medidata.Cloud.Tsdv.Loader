using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierFields")]
    public class TierField : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("FormOID")]
        public string FormOid { get; set; }

        [ColumnHeaderName("Fields")]
        public string Fields { get; set; }

        [ColumnHeaderName("IsLog")]
        public bool IsLog { get; set; }

        [ColumnHeaderName("tsdv_ControlType")]
        public string ControlType { get; set; }

        [ColumnHeaderName("tsdv_RequiresVerification")]
        public bool RequiresVerification { get; set; }
    }
}