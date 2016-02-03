using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierFields")]
    public class TierField : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("FormOid")]
        public string FormOid { get; set; }

        [ColumnHeaderName("FieldOid")]
        public string FieldOid { get; set; }

        [ColumnHeaderName("tsdv_Selected")]
        public bool Selected { get; set; }
    }
}