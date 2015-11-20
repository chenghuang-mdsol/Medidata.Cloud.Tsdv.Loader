using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ImpromptuInterface;
using Medidata.Cloud.Tsdv.Loader.ExcelConverters;
using Medidata.Cloud.Tsdv.Loader.Helpers;
using Medidata.Cloud.Tsdv.Loader.Models;
using Medidata.Interfaces.TSDV;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medidata.Cloud.Tsdv.Loader.Tests
{
    [TestClass]
    public class DemoTest
    {
        [TestMethod]
        public void Demo()
        {
            var blockPlan = new BlockPlanModel()
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
            };

            var bl = blockPlan.ActLike<IBlockPlan>();
            IModelConverterFactory modelConverterFactory = new ModelConverterFactory(new IModelConverter[]{});
            IExcelConverterFactory excelConverterFactory = new ExcelConverterFactory(new IExcelConverter[]{});

            IWorkbookBuilder builder = new WorkbookBuilder(modelConverterFactory,excelConverterFactory);
            var blockPlans = builder.EnsureWorksheet<IBlockPlan>("BlockPlan");
            
            blockPlans.Add(bl);
            using (SpreadsheetDocument doc = SpreadsheetDocument.Create(@"c:\temp\newtest.xlsx",SpreadsheetDocumentType.Workbook))
            {
                var wb = builder.ToWorkbook("TSDV WorkBook",doc);
                
            }
            
        }
    }
}