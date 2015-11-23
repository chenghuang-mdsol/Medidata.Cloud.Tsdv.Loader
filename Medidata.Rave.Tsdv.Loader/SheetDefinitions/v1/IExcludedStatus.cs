namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public interface IExcludedStatus
    {
        string SubjectStatus { get; }
        bool Excluded { get; }
    }
}