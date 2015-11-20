using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Medidata.Interfaces.TSDV;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medidata.Cloud.Tsdv.Loader.Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestOK()
        {
            var saver = new WorkbookSaver();



            var builder = new WorkbookBuilder(new ModelConverterFactory(null));
            builder.EnsureWorksheet<IBlockPlan>("Sheet1").Add(new BlockPlan { Name = "xxx" });
            builder.EnsureWorksheet<IBlockPlan>("Sheet1").Add(new BlockPlan { Name = "yyy" });

            var workbook = builder.ToWorkbook("workbook");

            var ms = new FileStream(@"C:\Github\test.xlsx", FileMode.Create);
            saver.SaveWorkbook(workbook, ms);
            ms.Close();

            Assert.IsNotNull(ms);
        }
    }

    public class BlockPlan : IBlockPlan
    {
        public string Name { get; set; }
        public int ObjectTypeId { get; set; }
        public int RandomizationType { get; private set; }
        public int ObjectId { get; set; }
        public int ParentObjectId { get; set; }
        public bool IsProdInUse { get; set; }
        public bool Deleted { get; set; }
        public IEnumerable<IBlock> Blocks { get; private set; }
        public IEnumerable<IBlockPlanEnvironment> BlockPlanEnvironments { get; private set; }
        public bool CanDelete { get; private set; }
        public int CRFVersionId { get; set; }
        public int MatrixId { get; set; }
        public decimal AverageSubjectPerSite { get; set; }
        public decimal CoveragePercent { get; set; }
        public DateTime DateEstimated { get; set; }
        public string ObjectName { get; set; }
        public int ActivatedByUser { get; set; }
        public bool Activated { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string ActivatedUserName { get; set; }
        public string MatrixName { get; set; }
        public string BlockPlanType { get; set; }
        public int ID { get; private set; }
        public bool Active { get; set; }
    }
}