using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medidata.Cloud.Tsdv.Loader.Attributes;
using Medidata.Interfaces.TSDV;

namespace Medidata.Cloud.Tsdv.Loader.ViewModels
{
    public class BlockPlan
    {
        [ExcelColumn(LocalizationKey = "tsdv_BlockPlanName")]
        public string Name { get; set; }
        [ExcelColumn(LocalizationKey = "tsdv_BlockPlanType")]
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

        public BlockPlan() { }
        public BlockPlan(IBlockPlan blockPlan)
        {
            var bp = blockPlan;
            Name = bp.Name;
            BlockPlanType = bp.BlockPlanType;
            ObjectName = bp.ObjectName;
            IsProdInUse = bp.IsProdInUse;
            RoleName = bp.RoleName;
            Activated = bp.Activated;
            ActivatedUserName = bp.ActivatedUserName;
            AverageSubjectPerSite = bp.AverageSubjectPerSite;
            CoveragePercent = bp.CoveragePercent;
            MatrixName = bp.MatrixName;
            DateEstimated = bp.DateEstimated;
        }

        public Dictionary<string, string> LocalizationDict { get; set; }
    }
}