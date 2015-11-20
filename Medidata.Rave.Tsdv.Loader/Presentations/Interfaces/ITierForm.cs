namespace Medidata.Rave.Tsdv.Loader.Presentations.Interfaces
{
    public interface ITierForm
    {
        string TierName { get; }
        string Forms { get; }
        string FormOid { get; }
        bool FieldsSelected { get; }
        bool FoldersSelected { get; }
    }
}