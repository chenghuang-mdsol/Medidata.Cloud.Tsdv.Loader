using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierFields")]
    public class TierField : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("tsdv_FormOid")]
        public string FormOid { get; set; }

        [ColumnHeaderName("tsdv_Fields")]
        public string Fields { get; set; }

        [ColumnHeaderName("tsdv_IsLog")]
        public bool IsLog { get; set; }

        [ColumnHeaderName("tsdv_ControlType")]
        public string ControlType { get; set; }

        [ColumnHeaderName("tsdv_RequiresVerification")]
        public bool RequiresVerification { get; set; }
    }
}