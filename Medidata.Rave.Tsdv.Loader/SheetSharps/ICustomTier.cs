namespace Medidata.Rave.Tsdv.Loader.SheetSharps
{
    public interface ICustomTier
    {
        string TierName { get; }
        string TierDescription { get; }
        bool LinkedToProdStudy { get; }
    }
}