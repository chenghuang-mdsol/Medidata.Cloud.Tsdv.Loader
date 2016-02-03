using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("RuleActions")]
    public class RuleAction : SheetModel
    {
        [ColumnHeaderName("tsdv_RuleName")]
        public string RuleName { get; set; }

        [ColumnHeaderName("tsdv_ActionName")]
        public string ActionName { get; set; }
    }
}