using System;
using System.IO;
using System.Linq;
using ImpromptuInterface;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.CellStyleProviders;
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
            var cellTypeValueConverterFactory = new CellTypeValueConverterFactory();
            var styleProvider = new ExtractedCellStyleProvider();
            // Use builder to create a .xlxs file
            var localizationService = ResolveLocalizationService();
            var loader = new TsdvReportV1Loader(cellTypeValueConverterFactory, localizationService, styleProvider);

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

            var filePath = @"C:\Github\test.xlsx";
            File.Delete(filePath);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                loader.Save(fs);
            }

            // Use parser to load a .xlxs file
            loader = new TsdvReportV1Loader(cellTypeValueConverterFactory, localizationService, styleProvider);
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