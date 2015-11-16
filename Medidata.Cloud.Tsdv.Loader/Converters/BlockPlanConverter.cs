using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medidata.Cloud.Tsdv.Loader.ViewModels;
using Medidata.Interfaces.TSDV;

namespace Medidata.Cloud.Tsdv.Loader.Converters
{
    public class BlockPlanConverter :IConverter 
    {
        public object ConvertBack(MiddleData data)
        {
            var rowData = data.RowData;
            var columnNames = data.ColumnNames;
            return new BlockPlan()
            {
                Name = rowData[columnNames.IndexFor("Block Plan Name")],
                BlockPlanType=rowData[columnNames.IndexFor("Block Plan Type")],
                ObjectName = rowData[columnNames.IndexFor("Study/SiteGroup/SiteName")],
                IsProdInUse = rowData[columnNames.IndexFor("Contains Subjects")].ToBoolean(),
                RoleName = rowData[columnNames.IndexFor("Data Entry Role")],
                Activated = rowData[columnNames.IndexFor("Block Plan Status")].ToBoolean("Active"),
                ActivatedUserName = rowData[columnNames.IndexFor("Plan Activated By")],
                AverageSubjectPerSite = rowData[columnNames.IndexFor("Average Subjects/Site")].ToDecimal(),
                CoveragePercent = rowData[columnNames.IndexFor("Estimated Coverage")].ToDecimal(),
                MatrixName = rowData[columnNames.IndexFor("Using Matrix")],
                DateEstimated = rowData[columnNames.IndexFor("Estimated Date")].ToDateTimeNullable()
            };

        }

        public MiddleData Convert(object obj)
        {
            IList<string> columnData = new List<string>()
            {
                "Block Plan Name",
                "Block Plan Type",
                "Study/SiteGroup/SiteName",
                "Contains Subjects",
                "Data Entry Role",
                "Block Plan Status",
                "Plan Activated By",
                "Average Subjects/Site",
                "Estimated Coverage",
                "Using Matrix",
                "Estimated Date"
            };
            if (!(obj is IBlockPlan))
            {
                return null;
            }
            IBlockPlan blockPlan = (IBlockPlan) obj;
            IList<string> rowData = new List<string>()
            {
                blockPlan.Name,
                blockPlan.BlockPlanType,
                blockPlan.ObjectName,
                blockPlan.IsProdInUse ? "Yes" : "No",
                blockPlan.RoleName,
                blockPlan.Activated ? "Active" : "InActive",
                blockPlan.ActivatedUserName,
                blockPlan.AverageSubjectPerSite.ToString(),
                blockPlan.CoveragePercent.ToString(),
                blockPlan.MatrixName,
                blockPlan.DateEstimated.ToString()
            };
            return new MiddleData(columnData,rowData);
        }
    }
}
