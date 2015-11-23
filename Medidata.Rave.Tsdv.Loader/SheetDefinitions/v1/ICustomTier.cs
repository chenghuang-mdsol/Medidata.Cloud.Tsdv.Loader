namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public interface ICustomTier
    {
        string TierName { get; }
        string TierDescription { get; }
        bool LinkedToProdStudy { get; }
    }
}