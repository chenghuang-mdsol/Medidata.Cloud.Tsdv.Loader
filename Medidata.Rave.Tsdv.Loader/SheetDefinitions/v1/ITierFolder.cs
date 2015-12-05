using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public interface ITierFolder : IDynamicFields
    {
        string TierName { get; }
        string FolderOid { get; }
    }
}