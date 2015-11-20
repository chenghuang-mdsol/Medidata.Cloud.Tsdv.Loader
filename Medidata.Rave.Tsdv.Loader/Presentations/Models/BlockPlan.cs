using System;
using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
{
    public class BlockPlan : IBlockPlan
    {
        public string BlockPlanName { get; set; }
        public string BlockPlanType { get; set; }
        public string StudyStudyGroupSiteName { get; set; }
        public string ContainsSubjects { get; set; }
        public string DataEntryRole { get; set; }
        public string BlockPlanStatus { get; set; }
        public string PlanActivatedBy { get; set; }
        public string AverageSubjectsSite { get; set; }
        public double EstimatedCoverage { get; set; }
        public bool UsingMatrix { get; set; }
        public DateTime? EstimatedDate { get; set; }
    }
}