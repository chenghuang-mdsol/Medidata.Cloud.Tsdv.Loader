using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("TierForms")]
    public class TierForm : SheetModel
    {
       public string TierName { get; set; }
        public string Forms { get; set; }
        public string FormOid { get; set; }
        public bool FieldsSelected { get; set; }
        public bool FoldersSelected { get; set; }
    }
}