using System;
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

            excelBuilderX.BlockPlans.Add(
                new BlockPlan
                {
                    BlockPlanName = "xxx",
                    UsingMatrix = false,
                    EstimatedDate = DateTime.Now,
                    EstimatedCoverage = 0.85
                },
                new BlockPlan {BlockPlanName = "yyy", EstimatedCoverage = 0.65},
                new BlockPlan {BlockPlanName = "zzz"});
            excelBuilderX.BlockPlanSettings.Add(
                new BlockPlanSetting
                {
                    BlockPlanName = "fakeNameByAnonymousClass",
                    Repeated = false,
                    BlockSubjectCount = 99
                },
                new BlockPlanSetting {BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100},
                new BlockPlanSetting {BlockPlanName = "ccc", Blocks = "fasdf"}
                );

            var sheetDef = excelBuilderX.GetSheetDefinition("TierFolders");
            sheetDef.AddColumn(new ColumnDefinition
                               {
                                   PropertyName = "Extra1",
                                   HeaderName = "tsdv_Extra1",
                                   PropertyType = typeof(string)
                               });


            dynamic tf = new TierFolder
                         {
                             TierName = "T1",
                             FolderOid = "FOLDETR"
                         };

            tf.Extra1 = "blah";

            excelBuilderX.TierFolders.Add((TierFolder) tf);

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