using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Rave.Tsdv.Loader.Validations.Rules;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;
using Medidata.Rave.Tsdv.Loader.Validations.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;


namespace Medidata.Rave.Tsdv.Loader.Tests.Validations.Rules
{
    [TestClass]
    public class ValidateBlockPlanSettingTiersTests
    {
        private IFixture _fixture;
        private ILocalization _localization;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _localization = _fixture.Create<ILocalization>();
        }

        [TestMethod]
        public void AllTiersAreSame()
        {
            // Arrange
            var tierNames = _fixture.CreateMany<string>().ToList();
            var customTiers = tierNames.Select(x => new CustomTier {TierName = x}).ToList();
            var extraColumns = tierNames.Select(x => new ColumnDefinition {ExtraProperty = true, PropertyName = x});

            var excelLoader = SetupExcelLoader(customTiers, extraColumns);

            // Act
            var sut = new ValidateBlockPlanSettingTiers(_localization);
            var result = sut.Check(excelLoader);

            // Assert
            Assert.IsFalse(result.Messages.Any());
            Assert.IsTrue(result.ShouldContinue);
        }

        [TestMethod]
        public void AllTiersAreDifferent()
        {
            // Arrange
            var tierNames = _fixture.CreateMany<string>().ToList();
            var customTiers = tierNames.Select(x => new CustomTier { TierName = x }).ToList();

            var tierNames2 = _fixture.CreateMany<string>().ToList();
            var extraColumns = tierNames2.Select(x => new ColumnDefinition { ExtraProperty = true, PropertyName = x });

            var excelLoader = SetupExcelLoader(customTiers, extraColumns);

            // Act
            var sut = new ValidateBlockPlanSettingTiers(_localization);
            var result = sut.Check(excelLoader);

            // Assert
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(result.Messages.Count, tierNames2.Count);
            Assert.IsFalse(result.ShouldContinue);
        }

        [TestMethod]
        public void OneBlockTierIsDifferent()
        {
            // Arrange
            var tierNames = _fixture.CreateMany<string>().ToList();
            var customTiers = tierNames.Select(x => new CustomTier { TierName = x }).ToList();

            tierNames[0] = _fixture.Create<string>();
            var extraColumns = tierNames.Select(x => new ColumnDefinition { ExtraProperty = true, PropertyName = x });

            var excelLoader = SetupExcelLoader(customTiers, extraColumns);

            // Act
            var sut = new ValidateBlockPlanSettingTiers(_localization);
            var result = sut.Check(excelLoader);

            // Assert
            Assert.IsTrue(result.Messages.Any()); 
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsFalse(result.ShouldContinue);
        }

        public IExcelLoader SetupExcelLoader(IList<CustomTier> customTiers, IEnumerable<IColumnDefinition> columns)
        {
            var sheetBlockPlanSetting = _fixture.Create<ISheetInfo<BlockPlanSetting>>();
            sheetBlockPlanSetting.Stub(x => x.Definition.ExtraColumnDefinitions)
                .Return(columns);

            var sheetCustomTier = _fixture.Create<ISheetInfo<CustomTier>>();
            sheetCustomTier.Stub(x => x.Data)
                .Return(customTiers);
            
            var excelLoader = _fixture.Create<IExcelLoader>();
            excelLoader.Stub(x => x.Sheet<BlockPlanSetting>())
                .Return(sheetBlockPlanSetting);
            excelLoader.Stub(x => x.Sheet<CustomTier>())
                .Return(sheetCustomTier);

            return excelLoader;
        }

    }
}
