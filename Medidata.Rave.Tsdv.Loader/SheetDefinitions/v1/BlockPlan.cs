using System;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("BlockPlans")]
    public class BlockPlan : SheetModel
    {
        [ColumnHeaderName("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }

        [ColumnHeaderName("tsdv_BlockPlanType")]
        public string BlockPlanType { get; set; }

        [ColumnHeaderName("tsdv_StSGSitNames")]
        public string StudyStudyGroupSiteName { get; set; }

        [ColumnHeaderName("tsdv_ContainsSubjects")]
        public string ContainsSubjects { get; set; }

        [ColumnHeaderName("tsdv_DataEntryRole")]
        public string DataEntryRole { get; set; }

        [ColumnHeaderName("tsdv_BlockPlanStatus")]
        public string BlockPlanStatus { get; set; }

        [ColumnHeaderName("tsdv_PlanActivatedBy")]
        public string PlanActivatedBy { get; set; }

        [ColumnHeaderName("tsdv_BlockPlanName")]
        public string AverageSubjectsSite { get; set; }

        [ColumnHeaderName("tsdv_EstimatedCoverage")]
        public double EstimatedCoverage { get; set; }

        [ColumnHeaderName("tsdv_UsingMatrix")]
        public bool UsingMatrix { get; set; }

        [ColumnHeaderName("tsdv_EstimatedDate")]
        public DateTime? EstimatedDate { get; set; }
    }
}