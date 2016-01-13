using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierFolders")]
    public class TierFolder : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("Form")]
        public string Form { get; set; }

        [ColumnHeaderName("FormOID")]
        public string FormOID { get; set; }
    }
}