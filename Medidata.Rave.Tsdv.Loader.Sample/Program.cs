using System;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.LoadConfiguration();

            var loader = container.Resolve<IExcelLoader>();

            // Case 1
            // Define a sheet by model type, and add items
            loader.Sheet<BlockPlan>();
            loader.Sheet<BlockPlan>().Data.Add(
                new BlockPlan
                {
                    BlockPlanName = "xxx",
                    UsingMatrix = false,
                    EstimatedDate = DateTime.Now,
                    EstimatedCoverage = 0.85
                },
                new BlockPlan {BlockPlanName = "yyy", EstimatedCoverage = 0.65},
                new BlockPlan {BlockPlanName = "zzz"});

            // Case 2
            // Automatically define sheet when initially calling SheetData with new type
            loader.Sheet<BlockPlanSetting>().Data.Add(
                new BlockPlanSetting
                {
                    BlockPlanName = "fakeNameByAnonymousClass",
                    Repeated = false,
                    BlockSubjectCount = 99
                },
                new BlockPlanSetting {BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100},
                new BlockPlanSetting {BlockPlanName = "ccc", Blocks = "fasdf"});

            // Case 3
            // Add dynamic columns and add extra properties to model object.
            loader.Sheet<TierFolder>().Definition
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

            loader.Sheet<TierFolder>().Data.Add(
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
            var loaderForLoading = container.Resolve<IExcelLoader>();
            using (var fs = new FileStream(filePathX, FileMode.Open))
            {
                loaderForLoading.Load(fs);
            }

            Console.WriteLine(loaderForLoading.Sheet<BlockPlan>().Data.First().BlockPlanName);
            Console.WriteLine(loaderForLoading.Sheet<BlockPlanSetting>().Data.Count);
            // Load extra properties from extra columns.
            Console.WriteLine(loaderForLoading.Sheet<TierFolder>().Data.First().GetExtraProperties()["Visit1"]);
            Console.WriteLine(loaderForLoading.Sheet<Rule>().Data.Count);

            Console.Read();
        }
    }
}