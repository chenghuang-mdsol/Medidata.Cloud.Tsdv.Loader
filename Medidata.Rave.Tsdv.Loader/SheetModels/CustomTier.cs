using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
{
    public class CustomTier : ICustomTier
    {
        public string TierName { get; set; }
        public string TierDescription { get; set; }
        public bool LinkedToProdStudy { get; set; }
    }
}