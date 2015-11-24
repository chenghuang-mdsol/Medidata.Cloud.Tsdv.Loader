using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;
using Medidata.Rave.Tsdv.Loader.Presentations.Models;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Download();
            Upload();
        }

        private static void Download()
        {
            var localizationService = ResolveJpnLocalizationService();
            var builder = new TsdvReportExcelBuilder(localizationService);

            var sheet = builder.AddSheet<IBlockPlan>("BlockPlan");
            sheet.Add(new BlockPlan
            {
                BlockPlanName = "Block Plan xxx",
                UsingMatrix = false,
                EstimatedDate = DateTime.Now,
                EstimatedCoverage = 0.85,
                BlockPlanType = "Study"
            });
            sheet.Add(new { BlockPlanName = "Block Plan yyy", EstimatedCoverage = 0.65, BlockPlanType = "Study" });
            sheet.Add(new { BlockPlanName = "Block Plan zzz", BlockPlanType = "Study Group" });

            //Translated into JPN
            var headers = new[] { "tsdv_BlockPlanName", "tsdv_Blocks", "tsdv_BlockSubjectCount", "tsdv_Repeated" };
            var sheet2 = builder.AddSheet<IBlockPlanSetting>("BlockPlanSetting", headers);
            sheet2.Add(new { BlockPlanName = "fakeNameByAnonymousClass", Repeated = false, BlockSubjectCount = 99 });
            sheet2.Add(new BlockPlanSetting { BlockPlanName = "Block Plan xxx", Repeated = true, BlockSubjectCount = 100 });
            sheet2.Add(new BlockPlanSetting { BlockPlanName = "Block Plan yyy", Blocks = "fasdf" });

            var filePath = @"C:\Github\test.xlsx";
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                builder.Save(fs);
            }
            Console.WriteLine("Excel file saved to {0}", filePath);
            
        }
        private static void Upload()
        {
            var filePath = @"C:\Github\test.xlsx";
            // Use parser to load a .xlsx file
            IList<IBlockPlan> blockPlans;
            IList<IBlockPlanSetting> blockPlanSettings;
            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var parser = new ExcelParser())
            {
                parser.Load(fs);

                blockPlans = parser.GetObjects<IBlockPlan>("BlockPlan").ToList();
                blockPlanSettings = parser.GetObjects<IBlockPlanSetting>("BlockPlanSetting").ToList();
            }
            Console.WriteLine("Excel file parsed from {0}", filePath);
            Console.WriteLine("Parsed BlockPlan");
            foreach (var blockPlan in blockPlans)
            {
                Console.WriteLine("Block Plan: {0} Parsed, Block Plan Type:{1}, EstimatedCoverage: {2}", blockPlan.BlockPlanName, blockPlan.BlockPlanType, blockPlan.EstimatedCoverage);
            }
            Console.WriteLine("Parsed BlockPlanSettings");
            foreach (var blockPlanSetting in blockPlanSettings)
            {
                Console.WriteLine("Block Plan Setting: {0} Parsed, Blocks:{1}, EstimatedCoverage: {2}", blockPlanSetting.BlockPlanName, blockPlanSetting.Blocks, blockPlanSetting.Repeated);
            }
            Console.ReadKey();
        }



        private static ILocalization ResolveLocalizationService()
        {
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            var localizationService = fixture.Create<ILocalization>();
            localizationService.Stub(x => x.GetLocalString(null))
                .IgnoreArguments()
                .Return(null)
                .WhenCalled(x =>
                {
                    var key = x.Arguments.First();
                    x.ReturnValue = "[" + key + "]";
                });
            return localizationService;
        }

        private static ILocalization ResolveJpnLocalizationService()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"tsdv_BlockPlanName", "ブロックプラン名"},
                {"tsdv_Blocks", "ブロック"},
                {"tsdv_BlockSubjectCount", "ブロックの被験者数"},
                {"tsdv_Repeated", "繰り返し"},
            };
            var fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            var localizationService = fixture.Create<ILocalization>();
            localizationService.Stub(x => x.GetLocalString(null)).IgnoreArguments()
                .Return(null).WhenCalled(x =>
            {
                var key = x.Arguments.First().ToString();
                string result = key.ToString();
                dict.TryGetValue(key, out result);
                x.ReturnValue = result;
            });
            return localizationService;
        }
    }
}