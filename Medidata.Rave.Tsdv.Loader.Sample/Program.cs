using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Medidata.Cloud.ExcelLoader;
using Medidata.Rave.Tsdv.Loader.SheetInterfaces;
using Medidata.Rave.Tsdv.Loader.SheetModels;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Use builder to create a .xlxs file
            var builder = new ExcelBuilder();

            var sheet = builder.AddSheet<IBlockPlan>("BlockPlan", true);
            sheet.Add(new BlockPlan { BlockPlanName = "xxx", UsingMatrix = false, EstimatedDate = DateTime.Now, EstimatedCoverage = 0.85 });
            sheet.Add(new { BlockPlanName = "yyy", EstimatedCoverage = 0.65 });
            sheet.Add(new { BlockPlanName = "zzz" });

            var headers = new[] { "Block Plan Name", "Blocks", "Block Subject Count", "Repeated" };
            var sheet2 = builder.AddSheet<IBlockPlanSetting>("BlockPlanSetting", headers);
            sheet2.Add(new { BlockPlanName = "fakeNameByAnonymousClass", Repeated = false, BlockSubjectCound = 99 });
            sheet2.Add(new BlockPlanSetting { BlockPlanName = "111", Repeated = true, BlockSubjectCound = 100});
            sheet2.Add(new BlockPlanSetting { BlockPlanName = "ccc", Blocks = "fasdf" });

            var filePath = @"C:\Github\test.xlsx";
            File.Delete(filePath);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                builder.Save(fs);
            }

            // Use parser to load a .xlxs file
            IList<IBlockPlan> blockPlans;
            IList<IBlockPlanSetting> blockPlanSettings;
            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var parser = new ExcelParser())
            {
                parser.Load(fs);

                blockPlans = parser.GetObjects<IBlockPlan>("BlockPlan").ToList();
                blockPlanSettings = parser.GetObjects<IBlockPlanSetting>("BlockPlanSetting").ToList();
            }

            Console.WriteLine(blockPlans.Count);
            Console.WriteLine(blockPlanSettings.Count);

            return;
        }


    }
}
