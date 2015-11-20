namespace Medidata.Rave.Tsdv.Loader.Presentations.Interfaces
{
    public interface ICustomTier
    {
        string TierName { get; }
        string TierDescription { get; }
        bool LinkedToProdStudy { get; }
    }
}