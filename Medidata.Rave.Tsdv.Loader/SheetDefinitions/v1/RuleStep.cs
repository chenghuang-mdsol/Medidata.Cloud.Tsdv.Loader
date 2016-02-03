using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("RuleSteps")]
    public class RuleStep : SheetModel
    {
        [ColumnHeaderName("tsdv_RuleName")]
        public string RuleName { get; set; }

        [ColumnHeaderName("tsdv_Ordinal")]
        public int Ordinal { get; set; }

        [ColumnHeaderName("tsdv_FolderOid")]
        public string FolderOid { get; set; }

        [ColumnHeaderName("tsdv_FormOid")]
        public string FormOid { get; set; }

        [ColumnHeaderName("tsdv_FieldOid")]
        public string FieldOid { get; set; }

        [ColumnHeaderName("tsdv_CheckFunction")]
        public string CheckFunction { get; set; }

        [ColumnHeaderName("tsdv_Value")]
        public string Value { get; set; }

        [ColumnHeaderName("tsdv_SubjectStatus")]
        public string SubjectStatus { get; set; }

        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }
    }
}