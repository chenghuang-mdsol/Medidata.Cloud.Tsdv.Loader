namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
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