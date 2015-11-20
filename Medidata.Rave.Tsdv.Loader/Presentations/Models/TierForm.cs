using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
{
    public class TierForm : ITierForm
    {
        public string TierName { get; set; }
        public string Forms { get; set; }
        public string FormOid { get; set; }
        public bool FieldsSelected { get; set; }
        public bool FoldersSelected { get; set; }
    }
}