using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{

    [SheetName("CustomTiers")]
    public class CustomTier : SheetModel
    {
        public string TierName { get; set; }
        public string TierDescription { get; set; }
        public bool LinkedToProdStudy { get; set; }
    }
}