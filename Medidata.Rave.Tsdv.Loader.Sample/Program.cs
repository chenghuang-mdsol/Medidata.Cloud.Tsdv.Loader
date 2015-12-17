using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
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
            var localizationService = ResolveLocalizationService();

            var cellTypeValueConverterFactory = new CellTypeValueConverterFactory();
            var excelBuilderX = new TsdvReportLoader(cellTypeValueConverterFactory, localizationService);

            excelBuilderX.BlockPlans.AddSimilarShape(
                new
                {
                    BlockPlanName = "xxx",
                    UsingMatrix = false,
                    EstimatedDate = DateTime.Now,
                    EstimatedCoverage = 0.85
                },
                new { BlockPlanName = "yyy", EstimatedCoverage = 0.65 },
                new { BlockPlanName = "zzz" });
            excelBuilderX.BlockPlanSettings.AddSimilarShape(
                new { BlockPlanName = "fakeNameByAnonymousClass", Repeated = false, BlockSubjectCount = 99 },
                new { BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100 },
                new { BlockPlanName = "ccc", Blocks = "fasdf" }
                );

            var sheetDef = excelBuilderX.GetSheetDefinition("TierFolders");
            sheetDef.ColumnDefinitions.Add(new ColumnDefinition
            {
                PropertyName = "Extra1",
                HeaderName = "Extra1",
                PropertyType = typeof(string)
            });

            excelBuilderX.TierFolders.AddSimilarShape(
                new { TierName ="T1", FolderOid = "FOLDETR", Extra1 = "blah"}
                );

            var filePathX = @"C:\Github\test.xlsx";
            File.Delete(filePathX);
            using (var fs = new FileStream(filePathX, FileMode.Create))
            {
                excelBuilderX.Save(fs);
            }

            // Use parser to load a .xlxs file
            var loader = new TsdvReportLoader(cellTypeValueConverterFactory, localizationService);
            using (var fs = new FileStream(filePathX, FileMode.Open))
            {
                loader.Load(fs);
            }

            Console.WriteLine(loader.BlockPlans.First().BlockPlanName);
            Console.WriteLine(loader.BlockPlanSettings.Count);
            Console.WriteLine(loader.TierFolders.Count);
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