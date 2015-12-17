using System.Dynamic;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public interface ITierFolder: IExtraProperty
    {
        string TierName { get; }
        string FolderOid { get; }
    }

}