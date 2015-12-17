using System;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions;
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
            var tsdvReportLoader = new TsdvReportLoader(cellTypeValueConverterFactory, localizationService);

            tsdvReportLoader.BlockPlans.Add(
                new BlockPlan
                {
                    BlockPlanName = "xxx",
                    UsingMatrix = false,
                    EstimatedDate = DateTime.Now,
                    EstimatedCoverage = 0.85
                },
                new BlockPlan {BlockPlanName = "yyy", EstimatedCoverage = 0.65},
                new BlockPlan {BlockPlanName = "zzz"});

            tsdvReportLoader.BlockPlanSettings.Add(
                new BlockPlanSetting
                {
                    BlockPlanName = "fakeNameByAnonymousClass",
                    Repeated = false,
                    BlockSubjectCount = 99
                },
                new BlockPlanSetting {BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100},
                new BlockPlanSetting {BlockPlanName = "ccc", Blocks = "fasdf"});


            tsdvReportLoader.GetSheetDefinition("TierFolders")
                            .AddColumn(new ColumnDefinition
                                       {
                                           PropertyName = "Visit1",
                                           PropertyType = typeof(bool)
                                       })
                            .AddColumn(new ColumnDefinition
                                       {
                                           PropertyName = "Visit2",
                                           PropertyType = typeof(int)
                                       })
                            .AddColumn(new ColumnDefinition
                                       {
                                           PropertyName = "Unscheduled",
                                           PropertyType = typeof(string)
                                       });

            tsdvReportLoader.TierFolders.Add(
                new TierFolder {TierName = "T1", FolderOid = "FOLDETR"}.AddProperty("Visit1", true),
                new TierFolder {TierName = "T1", FolderOid = "FOLDETR"}.AddProperty("Visit2", 1),
                new TierFolder {TierName = "T1", FolderOid = "FOLDETR"}.AddProperty("Unscheduled", "x"));

            var filePathX = @"C:\Github\test.xlsx";
            File.Delete(filePathX);
            using (var fs = new FileStream(filePathX, FileMode.Create))
            {
                tsdvReportLoader.Save(fs);
            }

            // Use parser to load a .xlxs file
            using (var fs = new FileStream(filePathX, FileMode.Open))
            {
                tsdvReportLoader.Load(fs);
            }

            Console.WriteLine(tsdvReportLoader.BlockPlans.First().BlockPlanName);
            Console.WriteLine(tsdvReportLoader.BlockPlanSettings.Count);
            Console.WriteLine(tsdvReportLoader.TierFolders[0].ExtraProperties["Visit1"]);
            Console.WriteLine(tsdvReportLoader.Rules.Count);

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