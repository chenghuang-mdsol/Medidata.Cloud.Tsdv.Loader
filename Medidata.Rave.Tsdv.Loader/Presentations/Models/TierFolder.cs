using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
{
    public class TierFolder : ITierFolder
    {
        public string TierName { get; set; }
        public string FolderOid { get; set; }
    }
}