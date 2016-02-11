using System;
using System.IO;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions;
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
            var loader = _container.Resolve<ITsdvExcelLoaderFactory>().Create(TsdvLoaderSupportedVersion.V1);

            Console.WriteLine("Loading from stream");
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                loader.Load(fs);
            }
            Console.WriteLine("Loaded");

            Console.WriteLine(loader.Sheet<BlockPlanSetting>().Data.Count);
            // Load extra properties from extra columns.
            Console.WriteLine(loader.Sheet<TierFormFolder>().Data[0].GetExtraProperties()["Visit1"]);
            Console.WriteLine(loader.Sheet<TierFormFolder>().Data[1].GetExtraProperties()["Visit2"]);
            Console.WriteLine(loader.Sheet<TierFormFolder>().Data[2].GetExtraProperties()["SomeDate"]);
            Console.WriteLine(loader.Sheet<TierFormFolder>().Data[3].GetExtraProperties()["Unscheduled"]);
            Console.WriteLine(loader.Sheet<Rule>().Data.Count);
        }

        private static void DownloadTsdvReport(string filePath)
        {
            var loader = _container.Resolve<ITsdvExcelLoaderFactory>().Create(TsdvLoaderSupportedVersion.V1);

            // Case 2
            // Automatically define sheet when initially calling SheetData with new type
            loader.Sheet<BlockPlanSetting>().Data.Add(
                new BlockPlanSetting
                {
                    Block = "fakeNameByAnonymousClass",
                    BlockSubjectCount = 99
                },
                new BlockPlanSetting { Block = "111", Repeated = true, BlockSubjectCount = 100 },
                new BlockPlanSetting { Block = "ccc"});

            loader.Sheet<TierFormField>().Data.Add(new TierFormField
            {
                FieldOid = "Visit2",
                FormOid = "VISIT",
                Selected = true,
                TierName = "Tier1"
            });
            // Case 3
            // Add dynamic columns and add extra properties to model object.
            loader.Sheet<TierFormFolder>().Definition
                  .AddColumn("Visit1")
                  .AddColumn("Visit2")
                  .AddColumn("SomeDate")
                  .AddColumn("Unscheduled");

            loader.Sheet<TierFormFolder>().Data.Add(
                new TierFormFolder { TierName = "T1", FormOid = "VISIT" }.AddProperty("Visit1", true),
                new TierFormFolder { TierName = "T2", FormOid = "VISIT" }.AddProperty("Visit2", 100),
                new TierFormFolder { TierName = "T3", FormOid = "SOMEDATE" }.AddProperty("SomeDate", new DateTime(1999, 4, 6)),
                new TierFormFolder { TierName = "T4", FormOid = "UNSCHEDULED" }.AddProperty("Unscheduled", "xxxxx"));

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