namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
{
    public interface ITierField
    {
        string TierName { get; }
        string FormOid { get; }
        string Fields { get; }
        bool IsLog { get; }
        string ControlType { get; }
        bool RequiresVerification { get; }
    }
}