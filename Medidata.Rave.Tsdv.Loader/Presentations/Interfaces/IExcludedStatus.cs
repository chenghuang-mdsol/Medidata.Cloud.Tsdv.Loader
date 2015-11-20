namespace Medidata.Rave.Tsdv.Loader.Presentations.Interfaces
{
    public interface IExcludedStatus
    {
        string SubjectStatus { get; }
        bool Excluded { get; }
    }
}