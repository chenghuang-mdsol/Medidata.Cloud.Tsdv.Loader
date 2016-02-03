using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("Rules")]
    public class Rule : SheetModel
    {
        [ColumnHeaderName("tsdv_Name")]
        public string Name { get; set; }

        [ColumnHeaderName("tsdv_Type")]
        public string Type { get; set; }

        [ColumnHeaderName("tsdv_RunRetrospective")]
        public bool RunsRetrospective { get; set; }

        [ColumnHeaderName("TSDVRuleWin_LabelBackfillSlotFlag")]
        public bool BackfillOpenSlots { get; set; }
    }
}