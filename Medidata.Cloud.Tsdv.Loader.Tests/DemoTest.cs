using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.Tsdv.Loader.Helpers;
using Medidata.Cloud.Tsdv.Loader.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medidata.Cloud.Tsdv.Loader.Tests
{
    [TestClass]
    public class DemoTest
    {
        [TestMethod]
        public void Demo1()
        {
            var blockPlans = new List<BlockPlan>
            {
                new BlockPlan
                {
                    Activated = true,
                    ActivatedUserName = "cheng",
                    AverageSubjectPerSite = 15,
                    BlockPlanType = "Study",
                    CoveragePercent = 95,
                    DateEstimated = DateTime.Now,
                    IsProdInUse = true,
                    MatrixName = "DefaultMatrix",
                    Name = "Plan A",
                    ObjectName = "ObjectName1",
                    RoleName = "Role 1"
                }
            };
            var tsdv = new TSDV();
            tsdv.BlockPlans = blockPlans;
            var helper = new ExcelHelper();
            helper.ConvertToExcelAndSave(@"c:\temp\tsdv_test.xlsx",tsdv);
        }

        [TestMethod]
        public void Demo2()
        {
            var helper = new ExcelHelper();
            TSDV tsdv = helper.ConvertFromExcel<TSDV>(@"TestHelpers\\tsdv_test.xlsx");
            Assert.IsNotNull(tsdv);
            Assert.IsNotNull(tsdv.BlockPlans);
            Assert.AreEqual(1,tsdv.BlockPlans.Count);
            Assert.AreEqual("Study", tsdv.BlockPlans[0].BlockPlanType);
        }
    }
}