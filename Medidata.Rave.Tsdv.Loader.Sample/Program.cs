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
            var loader = new TsdvReportLoader(cellTypeValueConverterFactory, localizationService);

            loader.AddOrGetSheetDefinition<BlockPlan>();
            loader.SheetData<BlockPlan>().Add(
                new BlockPlan
                {
                    BlockPlanName = "xxx",
                    UsingMatrix = false,
                    EstimatedDate = DateTime.Now,
                    EstimatedCoverage = 0.85
                },
                new BlockPlan {BlockPlanName = "yyy", EstimatedCoverage = 0.65},
                new BlockPlan {BlockPlanName = "zzz"});

//            loader.AddOrGetSheetDefinition<BlockPlanSetting>();
            loader.SheetData<BlockPlanSetting>().Add(
                new BlockPlanSetting
                {
                    BlockPlanName = "fakeNameByAnonymousClass",
                    Repeated = false,
                    BlockSubjectCount = 99
                },
                new BlockPlanSetting {BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100},
                new BlockPlanSetting {BlockPlanName = "ccc", Blocks = "fasdf"});


            loader.AddOrGetSheetDefinition<TierFolder>()
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

            loader.SheetData<TierFolder>().Add(
                new TierFolder {TierName = "T1", FolderOid = "FOLDETR"}.AddProperty("Visit1", true),
                new TierFolder {TierName = "T1", FolderOid = "FOLDETR"}.AddProperty("Visit2", 1),
                new TierFolder {TierName = "T1", FolderOid = "FOLDETR"}.AddProperty("Unscheduled", "x"));

            var filePathX = @"C:\Github\test.xlsx";
            File.Delete(filePathX);
            using (var fs = new FileStream(filePathX, FileMode.Create))
            {
                loader.Save(fs);
            }

            // Use parser to load a .xlxs file
            using (var fs = new FileStream(filePathX, FileMode.Open))
            {
                loader.Load(fs);
            }

            Console.WriteLine(loader.SheetData<BlockPlan>().First().BlockPlanName);
            Console.WriteLine(loader.SheetData<BlockPlanSetting>().Count);
            Console.WriteLine(loader.SheetData<TierFolder>().First().ExtraProperties["Visit1"]);
            Console.WriteLine(loader.SheetData<Rule>().Count);

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