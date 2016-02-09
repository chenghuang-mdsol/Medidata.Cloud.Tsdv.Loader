using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("Rules")]
    public class Rule : SheetModel
    {
        [ColumnInfo("Name")]
        public string Name { get; set; }

        [ColumnInfo("Type")]
        public string Type { get; set; }

        [ColumnInfo("Step")]
        public string Step { get; set; }

        [ColumnInfo("Action")]
        public string Action { get; set; }

        [ColumnInfo("tsdv_RunRetrospective")]
        public bool RunsRetrospective { get; set; }

        [ColumnInfo("TSDVRuleWin_LabelBackfillSlotFlag")]
        public bool BackfillOpenSlots { get; set; }

        [ColumnInfo("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }
    }
}