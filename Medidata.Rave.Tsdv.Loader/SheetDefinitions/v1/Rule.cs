using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [ExcludeFromCodeCoverage]
    [SheetName("Rules")]
    public class Rule : SheetModel
    {
        [ColumnHeaderName("tsdv_Name")]
        public string Name { get; set; }

        [ColumnHeaderName("tsdv_LinkedToPlan")]
        public bool LinkedToPlan { get; set; }

        [ColumnHeaderName("tsdv_Type")]
        public string Type { get; set; }

        [ColumnHeaderName("tsdv_RunRetrospective")]
        public bool RunsRetrospective { get; set; }

        [ColumnHeaderName("TSDVRuleWin_LabelBackfillSlotFlag")]
        public bool BackfillOpenSlots { get; set; }

        [ColumnHeaderName("tsdv_FolderOid")]
        public string FolderOid { get; set; }

        [ColumnHeaderName("tsdv_FormOid")]
        public string FormOid { get; set; }

        [ColumnHeaderName("tsdv_FieldOid")]
        public string FieldOid { get; set; }

        [ColumnHeaderName("tsdv_SubjectStatus")]
        public string SubjectStatus { get; set; }

        [ColumnHeaderName("tsdv_StepCondition")]
        public string StepCondition { get; set; }

        [ColumnHeaderName("tsdv_StepValue")]
        public string StepValue { get; set; }

        [ColumnHeaderName("tsdv_TierStepJoin")]
        public string TierStepJoin { get; set; }

        [ColumnHeaderName("tsdv_TierStepCondition")]
        public string TierStepCondition { get; set; }

        [ColumnHeaderName("tsdv_TierStepTargetTier")]
        public string TierStepTargetTier { get; set; }

        [ColumnHeaderName("tsdv_ActionName")]
        public string ActionName { get; set; }

        [ColumnHeaderName("tsdv_ActionTargetTier")]
        public string ActionTargetTier { get; set; }
    }
}