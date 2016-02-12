using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [ExcludeFromCodeCoverage]
    [SheetName("Rules")]
    public class Rule : SheetModel
    {
        [ColumnInfo("tsdv_Name")]
        public string Name { get; set; }

        [ColumnInfo("tsdv_LinkedToPlan")]
        public bool LinkedToPlan { get; set; }

        [ColumnInfo("tsdv_Type")]
        public string Type { get; set; }

        [ColumnInfo("tsdv_RunRetrospective")]
        public bool RunsRetrospective { get; set; }

        [ColumnInfo("TSDVRuleWin_LabelBackfillSlotFlag")]
        public bool BackfillOpenSlots { get; set; }

        [ColumnInfo("tsdv_FolderOid")]
        public string FolderOid { get; set; }

        [ColumnInfo("tsdv_FormOid")]
        public string FormOid { get; set; }

        [ColumnInfo("tsdv_FieldOid")]
        public string FieldOid { get; set; }

        [ColumnInfo("tsdv_SubjectStatus", "SubjectStatusSource")]
        public string SubjectStatus { get; set; }

        [ColumnInfo("tsdv_StepCondition")]
        public string StepCondition { get; set; }

        [ColumnInfo("tsdv_StepValue")]
        public string StepValue { get; set; }

        [ColumnInfo("tsdv_TierStepJoin")]
        public string TierStepJoin { get; set; }

        [ColumnInfo("tsdv_TierStepCondition")]
        public string TierStepCondition { get; set; }

        [ColumnInfo("tsdv_TierStepTargetTier")]
        public string TierStepTargetTier { get; set; }

        [ColumnInfo("tsdv_ActionName")]
        public string ActionName { get; set; }

        [ColumnInfo("tsdv_ActionTargetTier")]
        public string ActionTargetTier { get; set; }
    }
}