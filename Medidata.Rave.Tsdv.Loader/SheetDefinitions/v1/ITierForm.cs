namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
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