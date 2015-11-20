using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
{
    public class CustomTier : ICustomTier
    {
        public string TierName { get; set; }
        public string TierDescription { get; set; }
        public bool LinkedToProdStudy { get; set; }
    }
}