using System;
using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("BlockPlans")]
    public class BlockPlan : SheetModel
    {
        [ColumnInfo("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }

        [ColumnInfo("tsdv_BlockPlanType")]
        public string BlockPlanType { get; set; }

        [ColumnInfo("tsdv_StSGSitNames")]
        public string StudyStudyGroupSiteName { get; set; }

        [ColumnInfo("tsdv_ContainsSubjects")]
        public string ContainsSubjects { get; set; }

        [ColumnInfo("tsdv_DataEntryRole")]
        public string DataEntryRole { get; set; }

        [ColumnInfo("tsdv_BlockPlanStatus")]
        public string BlockPlanStatus { get; set; }

        [ColumnInfo("tsdv_PlanActivatedBy")]
        public string PlanActivatedBy { get; set; }

        [ColumnInfo("tsdv_BlockPlanName")]
        public string AverageSubjectsSite { get; set; }

        [ColumnInfo("tsdv_EstimatedCoverage")]
        public double EstimatedCoverage { get; set; }

        [ColumnInfo("tsdv_UsingMatrix")]
        public bool UsingMatrix { get; set; }

        [ColumnInfo("tsdv_EstimatedDate")]
        public DateTime? EstimatedDate { get; set; }
    }
}