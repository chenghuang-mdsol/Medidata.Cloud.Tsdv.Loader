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
            loader.SheetDefinition<BlockPlan>();
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

            // Case 2
            // Automatically define sheet when initially calling SheetData with new type
            loader.SheetData<BlockPlanSetting>().Add(
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
            loader.SheetDefinition<TierFolder>()
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
            // Load extra properties from extra columns.
            Console.WriteLine(loader.SheetData<TierFolder>().First().GetExtraProperties()["Visit1"]);
            Console.WriteLine(loader.SheetData<Rule>().Count);

            Console.Read();
        }
    }
}