using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImpromptuInterface;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            var excelBuilderX = new TsdvReportLoader(null);

            excelBuilderX.BlockPlans.AddAnonymous(
                new
                {
                    BlockPlanName = "xxx",
                    UsingMatrix = false,
                    EstimatedDate = DateTime.Now,
                    EstimatedCoverage = 0.85
                },
                new { BlockPlanName = "yyy", EstimatedCoverage = 0.65 }, 
                new { BlockPlanName = "zzz" });
            excelBuilderX.BlockPlanSettings.AddAnonymous(
                new { BlockPlanName = "fakeNameByAnonymousClass", Repeated = false, BlockSubjectCount = 99 },
                new { BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100 },
                new { BlockPlanName = "ccc", Blocks = "fasdf" }
                );

            var filePathX = @"C:\Github\test.xlsx";
            File.Delete(filePathX);
            using (var fs = new FileStream(filePathX, FileMode.Create))
            {
                excelBuilderX.Save(fs);
            }
//            return;
            // Use builder to create a .xlxs file
            var localizationService = ResolveLocalizationService();
            var loader = new TsdvReportLoader(localizationService);

            // Use parser to load a .xlxs file
            loader = new TsdvReportLoader(localizationService);
            using (var fs = new FileStream(filePathX, FileMode.Open))
            {
                loader.Load(fs);
            }

            Console.WriteLine(loader.BlockPlans.Count);
            Console.WriteLine(loader.BlockPlanSettings.Count);
            Console.WriteLine(loader.Rules.Count);

            Console.Read();
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