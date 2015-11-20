using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
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