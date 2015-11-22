using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImpromptuInterface;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetSharps;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cellTypeValueConverterFactory = new CellTypeValueConverterFactory();
            // Use builder to create a .xlxs file
            var localizationService = ResolveLocalizationService();
            var builder = new TsdvReportExcelBuilder(cellTypeValueConverterFactory, localizationService);

            var sheet = builder.AddSheet<IBlockPlan>("BlockPlan");
            sheet.Add(new 
            {
                BlockPlanName = "xxx",
                UsingMatrix = false,
                EstimatedDate = DateTime.Now,
                EstimatedCoverage = 0.85
            }.ActLike<IBlockPlan>());
            sheet.Add(new { BlockPlanName = "yyy", EstimatedCoverage = 0.65 }.ActLike<IBlockPlan>());
            sheet.Add(new { BlockPlanName = "zzz" }.ActLike<IBlockPlan>());

            var headers = new[] {"tsdv_BlockPlanName", "tsdv_Blocks", "tsdv_BlockSubjectCount", "tsdv_Repeated"};
            var sheet2 = builder.AddSheet<IBlockPlanSetting>("BlockPlanSetting", headers);
            sheet2.Add(new { BlockPlanName = "fakeNameByAnonymousClass", Repeated = false, BlockSubjectCound = 99 }.ActLike<IBlockPlanSetting>());
            sheet2.Add(new {BlockPlanName = "111", Repeated = true, BlockSubjectCound = 100}.ActLike<IBlockPlanSetting>());
            sheet2.Add(new {BlockPlanName = "ccc", Blocks = "fasdf"}.ActLike<IBlockPlanSetting>());

            var filePath = @"C:\Github\test.xlsx";
            File.Delete(filePath);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                builder.Save(fs);
            }

            // Use parser to load a .xlxs file
            IList<IBlockPlan> blockPlans;
            IList<IBlockPlanSetting> blockPlanSettings;
            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var parser = new ExcelParser(cellTypeValueConverterFactory))
            {
                parser.Load(fs);

                blockPlans = parser.GetObjects<IBlockPlan>("BlockPlan").ToList();
                blockPlanSettings = parser.GetObjects<IBlockPlanSetting>("BlockPlanSetting").ToList();
            }

            Console.WriteLine(blockPlans.Count);
            Console.WriteLine(blockPlanSettings.Count);
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
    }
}