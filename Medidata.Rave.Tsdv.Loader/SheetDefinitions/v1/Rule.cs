using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("Rules")]
    public class Rule : SheetModel
    {
        [ColumnHeaderName("Name")]
        public string Name { get; set; }

        [ColumnHeaderName("Type")]
        public string Type { get; set; }

        [ColumnHeaderName("Step")]
        public string Step { get; set; }

        [ColumnHeaderName("Action")]
        public string Action { get; set; }

        [ColumnHeaderName("tsdv_RunsRetrospective")]
        public bool RunsRetrospective { get; set; }

        [ColumnHeaderName("TSDVRuleWin_LabelBackfillSlotFlag")]
        public bool BackfillOpenSlots { get; set; }

        [ColumnHeaderName("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }
    }
}