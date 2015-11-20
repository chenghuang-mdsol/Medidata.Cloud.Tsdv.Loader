using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
{
    public class TierField : ITierField
    {
        public string TierName { get; set; }
        public string FormOid { get; set; }
        public string Fields { get; set; }
        public bool IsLog { get; set; }
        public string ControlType { get; set; }
        public bool RequiresVerification { get; set; }
    }
}