namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
{
    public interface IExcludedStatus
    {
        string SubjectStatus { get; }
        bool Excluded { get; }
    }
}