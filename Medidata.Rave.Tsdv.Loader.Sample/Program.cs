﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImpromptuInterface;
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
            // Use builder to create a .xlxs file
            var localizationService = ResolveLocalizationService();
            var loader = new TsdvReportLoader(localizationService);
            
            loader.BlockPlans.Add(new
            {
                BlockPlanName = "xxx",
                UsingMatrix = false,
                EstimatedDate = DateTime.Now,
                EstimatedCoverage = 0.85
            }.ActLike<IBlockPlan>());
            loader.BlockPlans.Add(new {BlockPlanName = "yyy", EstimatedCoverage = 0.65}.ActLike<IBlockPlan>());
            loader.BlockPlans.Add(new {BlockPlanName = "zzz"}.ActLike<IBlockPlan>());

            loader.BlockPlanSettings.Add(
                new {BlockPlanName = "fakeNameByAnonymousClass", Repeated = false, BlockSubjectCount = 99}
                    .ActLike<IBlockPlanSetting>());
            loader.BlockPlanSettings.Add(
                new {BlockPlanName = "111", Repeated = true, BlockSubjectCount = 100}.ActLike<IBlockPlanSetting>());
            loader.BlockPlanSettings.Add(new {BlockPlanName = "ccc", Blocks = "fasdf"}.ActLike<IBlockPlanSetting>());
            
            loader.TierFolders.Add(new {TierName = "Folder1", FolderOid = "id001", DynamicFields = new List<string>(){"x","y","z"}, DynamicColumnNames = new string[]{"Screening", "30 days", "60 days"}}.ActLike<ITierFolder>());
            var filePath = @"C:\Github\test.xlsx";
            File.Delete(filePath);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                loader.Save(fs);
            }

            // Use parser to load a .xlxs file
            loader = new TsdvReportLoader(localizationService);
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                loader.Load(fs);
            }

            Console.WriteLine(loader.BlockPlans.Count);
            Console.WriteLine(loader.BlockPlanSettings.Count);
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