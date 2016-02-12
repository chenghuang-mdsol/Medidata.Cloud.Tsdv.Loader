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
    public class BlockPlanSettingSheetShouldHaveMoreThanZeroTiersTests
    {
        private IExcelLoader _loader;
        private BlockPlanSettingSheetShouldHaveMoreThanZeroTiers _sut;

        [TestInitialize]
        public void Init()
        {
            var localization = TestHelper.CreateStubLocalization();
            _sut = new BlockPlanSettingSheetShouldHaveMoreThanZeroTiers(localization);
            _loader = this.GetFixture().Create<IExcelLoader>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new BlockPlanSettingSheetShouldHaveMoreThanZeroTiers(null);
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenTierCountIsZero()
        {
            _loader.Stub(x => x.Sheet<BlockPlanSetting>())
                   .Return(this.GetFixture().Create<ISheetInfo<BlockPlanSetting>>());

            var blockPlanSettings = new[]
                                    {
                                        new BlockPlanSetting()
                                    };
            _loader.Sheet<BlockPlanSetting>().Stub(x => x.Data).Return(blockPlanSettings);

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenTierCountIsNotZero()
        {
            _loader.Stub(x => x.Sheet<BlockPlanSetting>())
                   .Return(this.GetFixture().Create<ISheetInfo<BlockPlanSetting>>());

            var blockPlanSetting = new BlockPlanSetting ();
            dynamic blockPlanSettingDyn = blockPlanSetting;
            blockPlanSettingDyn.ExtraProp1 = 0;
            blockPlanSettingDyn.ExtraProp2 = 0;

            _loader.Sheet<BlockPlanSetting>().Stub(x => x.Data).Return(new[] {blockPlanSetting});

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenBlockSubjectCountMatched()
        {
            _loader.Stub(x => x.Sheet<BlockPlanSetting>())
                   .Return(this.GetFixture().Create<ISheetInfo<BlockPlanSetting>>());

            var blockPlanSetting = new BlockPlanSetting ();
            dynamic blockPlanSettingDyn = blockPlanSetting;
            blockPlanSettingDyn.ExtraProp1 = 1;
            blockPlanSettingDyn.ExtraProp2 = 2;

            _loader.Sheet<BlockPlanSetting>().Stub(x => x.Data).Return(new[] {blockPlanSetting});

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }
    }
}