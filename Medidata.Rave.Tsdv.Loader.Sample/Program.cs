﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Rave.Tsdv.Loader.ColumnResources;
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
            Console.WriteLine(loader.Sheet<TierFolder>().Data[0].GetExtraProperties()["Visit1"]);
            Console.WriteLine(loader.Sheet<TierFolder>().Data[1].GetExtraProperties()["Visit2"]);
            Console.WriteLine(loader.Sheet<TierFolder>().Data[2].GetExtraProperties()["SomeDate"]);
            Console.WriteLine(loader.Sheet<TierFolder>().Data[3].GetExtraProperties()["Unscheduled"]);
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

            loader.Sheet<TierField>().Data.Add(new TierField
            {
                FieldOid = "Form1Field1",
                FormOid = "Form1",
                Selected = true,
                TierName = "Tier1"
            });
            // Case 3
            // Add dynamic columns and add extra properties to model object.
            loader.Sheet<TierFolder>().Definition
                  .AddColumn("Visit1")
                  .AddColumn("Visit2")
                  .AddColumn("SomeDate")
                  .AddColumn("Unscheduled");

            loader.Sheet<TierFolder>().Data.Add(
                new TierFolder { TierName = "T1", FormOid = "VISIT" }.AddProperty("Visit1", true),
                new TierFolder { TierName = "T2", FormOid = "VISIT" }.AddProperty("Visit2", 100),
                new TierFolder { TierName = "T3", FormOid = "SOMEDATE" }.AddProperty("SomeDate", new DateTime(1999, 4, 6)),
                new TierFolder { TierName = "T4", FormOid = "UNSCHEDULED" }.AddProperty("Unscheduled", "xxxxx"));

            File.Delete(filePath);
            Console.WriteLine("Saving into stream");
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                loader.Save(fs);
            }
            Console.WriteLine("Saved");
        }


    }

    public class SampleColumnResourceManager : IColumnResourceManager
    {
        public List<ColumnResource> Resources { get; set; }

        public SampleColumnResourceManager()
        {
            Resources = new List<ColumnResource>()
            {
                new ColumnResource()
                {
                    ResourceName = "Forms",
                    DependingKey = null,
                    List = new List<ColumnResourceItem>()
                    {
                        new ColumnResourceItem {Category = "FormOIDSource", Value = "Form1"},
                        new ColumnResourceItem {Category = "FormOIDSource", Value = "Form2"},
                        new ColumnResourceItem {Category = "FormOIDSource", Value = "Form3"}
                    }
                },
                new ColumnResource()
                {
                    ResourceName = "FormFields",
                    DependingKey = null,
                    List = new List<ColumnResourceItem>()
                    {
                        new ColumnResourceItem {Category = "FieldOid.Form1", Value = "Form1Field1"},
                        new ColumnResourceItem {Category = "FieldOid.Form2", Value = "Form2Field2"},
                        new ColumnResourceItem {Category = "FieldOid.Form3", Value = "Form3Field2"},
                        new ColumnResourceItem {Category = "FieldOid.Form1", Value = "Form1Field2"},
                        new ColumnResourceItem {Category = "FieldOid.Form1", Value = "Form1Field3"},

                        new ColumnResourceItem {Category = "FieldOid.Form2", Value = "Form2Field1"},
                        new ColumnResourceItem {Category = "FieldOid.Form3", Value = "Form3Field1"},
                        new ColumnResourceItem {Category = "FieldOid.Form2", Value = "Form2Field3"},
                    }
                },
            };
        }
    }
}