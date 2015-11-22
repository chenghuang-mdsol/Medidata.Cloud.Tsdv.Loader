namespace Medidata.Rave.Tsdv.Loader.SheetSharps
{
    public interface IExcludedStatus
    {
        string SubjectStatus { get; }
        bool Excluded { get; }
    }
}