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
        private static UnityContainer _container;
        
        private static void Main(string[] args)
        {
            _container = new UnityContainer();
            _container.LoadConfiguration();

            const string demoFilePath = @"C:\Github\test.xlsx";

            // Downloading demo (objects => Excel)
            DownloadTsdvReport(demoFilePath);

            // Uploading demo (Excel => objects)
            UploadTsdvReport(demoFilePath);

            Console.Read();
        }

        private static void UploadTsdvReport(string filePath)
        {
            var loader = _container.Resolve<IExcelLoader>();

            Console.WriteLine("Loading from stream");
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                loader.Load(fs);
            }
            Console.WriteLine("Loaded");

            Console.WriteLine(loader.Sheet<BlockPlan>().Data.First().BlockPlanName);
            Console.WriteLine(loader.Sheet<BlockPlanSetting>().Data.Count);
            // Load extra properties from extra columns.
            Console.WriteLine(loader.Sheet<TierFolder>().Data[0].GetExtraProperties()["Visit1"]);
            Console.WriteLine(loader.Sheet<TierFolder>().Data[1].GetExtraProperties()["Visit2"]);
            Console.WriteLine(loader.Sheet<TierFolder>().Data[2].GetExtraProperties()["SomeDate"]);
            Console.WriteLine(loader.Sheet<TierFolder>().Data[3].GetExtraProperties()["Unscheduled"]);
            Console.WriteLine(loader.Sheet<Rule>().Data.Count);
        }

        private static void DownloadTsdvReport(string filePath)
        {
            var loader = _container.Resolve<IExcelLoader>();

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
                new BlockPlan { BlockPlanName = "yyy", EstimatedCoverage = 0.65 },
                new BlockPlan { BlockPlanName = "zzz" });

            // Case 2
            // Automatically define sheet when initially calling SheetData with new type
            loader.Sheet<BlockPlanSetting>().Data.Add(
                new BlockPlanSetting
                {
                    BlockPlanName = "fakeNameByAnonymousClass",
                    Repeated = false,
                    BlockSubjectCount = 99
                },
                new BlockPlanSetting { BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100 },
                new BlockPlanSetting { BlockPlanName = "ccc", Blocks = "fasdf" });

            // Case 3
            // Add dynamic columns and add extra properties to model object.
            loader.Sheet<TierFolder>().Definition
                  .AddColumn("Visit1")
                  .AddColumn("Visit2")
                  .AddColumn("SomeDate")
                  .AddColumn("Unscheduled");

            loader.Sheet<TierFolder>().Data.Add(
                new TierFolder { TierName = "T1", FolderOid = "VISIT" }.AddProperty("Visit1", true),
                new TierFolder { TierName = "T2", FolderOid = "VISIT" }.AddProperty("Visit2", 100),
                new TierFolder { TierName = "T3", FolderOid = "SOMEDATE" }.AddProperty("SomeDate", new DateTime(1999, 4, 6)),
                new TierFolder { TierName = "T4", FolderOid = "UNSCHEDULED" }.AddProperty("Unscheduled", "xxxxx"));

            File.Delete(filePath);
            Console.WriteLine("Saving into stream");
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                loader.Save(fs);
            }
            Console.WriteLine("Saved");
        }
    }
}