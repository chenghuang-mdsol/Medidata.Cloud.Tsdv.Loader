using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("TierFolders")]
    public class TierFolder : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("Form", "Form.FormOID")]
        public string Form { get; set; }

        [ColumnInfo("FormOID", "FormOIDSource")]
        public string FormOID { get; set; }
    }
}