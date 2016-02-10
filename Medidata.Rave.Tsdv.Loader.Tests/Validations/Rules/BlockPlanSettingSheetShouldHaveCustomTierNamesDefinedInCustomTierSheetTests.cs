using System;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;
using Medidata.Rave.Tsdv.Loader.Tests.TestHelpers;
using Medidata.Rave.Tsdv.Loader.Validations.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests.Validations.Rules
{
    [TestClass]
    public class BlockPlanSettingSheetShouldHaveCustomTierNamesDefinedInCustomTierSheetTests
    {
        private IExcelLoader _loader;
        private BlockPlanSettingSheetShouldHaveCustomTierNamesDefinedInCustomTierSheet _sut;

        [TestInitialize]
        public void Init()
        {
            var localization = TestHelper.CreateStubLocalization();
            _sut = new BlockPlanSettingSheetShouldHaveCustomTierNamesDefinedInCustomTierSheet(localization);
            _loader = this.GetFixture().Create<IExcelLoader>();

            StubCustomTierSheetData(_loader);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new BlockPlanSettingSheetShouldHaveCustomTierNamesDefinedInCustomTierSheet(null);
        }

        private void StubCustomTierSheetData(IExcelLoader loader)
        {
            loader.Stub(x => x.Sheet<CustomTier>())
                  .Return(this.GetFixture().Create<ISheetInfo<CustomTier>>());

            var customDatas = new[]
                              {
                                  new CustomTier
                                  {
                                      TierName = "CustomTier1",
                                      TierDescription = this.GetFixture().Create<string>()
                                  },
                                  new CustomTier
                                  {
                                      TierName = "CustomTier2",
                                      TierDescription = this.GetFixture().Create<string>()
                                  }
                              };
            loader.Sheet<CustomTier>().Stub(x => x.Data).Return(customDatas);
        }

        private void StubBlockPlanSettingsExtraColumns(IExcelLoader loader, string extraPropName)
        {
            loader.Stub(x => x.Sheet<BlockPlanSetting>())
                  .Return(this.GetFixture().Create<ISheetInfo<BlockPlanSetting>>());
            loader.Sheet<BlockPlanSetting>()
                  .Stub(x => x.Definition)
                  .Return(this.GetFixture().Create<ISheetDefinition>());

            var colDefs = new[]
                          {
                              new ColumnDefinition {ExtraProperty = false, PropertyName = "Col1"},
                              new ColumnDefinition {ExtraProperty = false, PropertyName = "Col2"},
                              new ColumnDefinition {ExtraProperty = true, PropertyName = extraPropName}
                          };
            loader.Sheet<BlockPlanSetting>()
                  .Definition
                  .Stub(x => x.ColumnDefinitions)
                  .Return(colDefs);
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenTierNameIsArchitectDefined()
        {
            StubBlockPlanSettingsExtraColumns(_loader, "Architect Defined");

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenTierNameIsNoForms()
        {
            StubBlockPlanSettingsExtraColumns(_loader, "No Forms");

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenTierNameIsAllForms()
        {
            StubBlockPlanSettingsExtraColumns(_loader, "All Forms");

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenTierNameIsInCustomTiers()
        {
            StubBlockPlanSettingsExtraColumns(_loader, "CustomTier2");

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenTierNameIsNotInCustomTiers()
        {
            var badTierName = this.GetFixture().Create<string>();
            StubBlockPlanSettingsExtraColumns(_loader, badTierName);

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
            Assert.IsTrue(result.Messages.First().Message.Contains(badTierName));
        }
    }
}