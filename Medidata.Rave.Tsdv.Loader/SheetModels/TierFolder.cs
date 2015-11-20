using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
{
    public class TierFolder : ITierFolder
    {
        public string TierName { get; set; }
        public string FolderOid { get; set; }
    }
}