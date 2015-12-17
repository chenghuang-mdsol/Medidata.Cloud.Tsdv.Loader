using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierFolders")]
    public class TierFolder : SheetModel
    {
        public string TierName { get; set; }
        public string FolderOid { get; set; }
    }
}