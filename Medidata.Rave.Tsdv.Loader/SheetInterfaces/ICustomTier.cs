namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
{
    public interface ICustomTier
    {
        string TierName { get; }
        string TierDescription { get; }
        bool LinkedToProdStudy { get; }
    }
}