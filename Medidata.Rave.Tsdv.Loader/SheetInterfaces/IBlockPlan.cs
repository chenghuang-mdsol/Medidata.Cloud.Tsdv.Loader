using System;

namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
{
    public interface IBlockPlan
    {
        string BlockPlanName { get; }
        string BlockPlanType { get; }
        string StudyStudyGroupSiteName { get; }
        string ContainsSubjects { get; }
        string DataEntryRole { get; }
        string BlockPlanStatus { get; }
        string PlanActivatedBy { get; }
        string AverageSubjectsSite { get; }
        double EstimatedCoverage { get; }
        bool UsingMatrix { get; }
        DateTime? EstimatedDate { get; }
    }
}