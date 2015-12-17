using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierFields")]
    public class TierField : SheetModel
    {
       public string TierName { get; set; }
        public string FormOid { get; set; }
        public string Fields { get; set; }
        public bool IsLog { get; set; }
        public string ControlType { get; set; }
        public bool RequiresVerification { get; set; }
    }
}