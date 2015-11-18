using System;

namespace Medidata.Cloud.Tsdv.Loader.Models
{
    public class BlockPlanModel
    {
        public string Name { get; set; }
        public string BlockPlanType { get; set; }
        public string ObjectName { get; set; }
        public bool IsProdInUse { get; set; }
        public string RoleName { get; set; }
        public bool Activated { get; set; }
        public string ActivatedUserName { get; set; }
        public decimal AverageSubjectPerSite { get; set; }
        public decimal CoveragePercent { get; set; }
        public string MatrixName { get; set; }
        public DateTime? DateEstimated { get; set; }
    }
}