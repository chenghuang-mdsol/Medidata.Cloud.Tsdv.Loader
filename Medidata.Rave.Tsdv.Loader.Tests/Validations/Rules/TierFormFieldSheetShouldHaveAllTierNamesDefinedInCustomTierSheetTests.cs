﻿using System;
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
    public class TierFormFieldSheetShouldHaveAllTierNamesDefinedInCustomTierSheetTests
    {
        private IExcelLoader _loader;
        private TierFormFieldSheetShouldHaveAllTierNamesDefinedInCustomTierSheet _sut;

        [TestInitialize]
        public void Init()
        {
            var localization = TestHelper.CreateStubLocalization();
            _sut = new TierFormFieldSheetShouldHaveAllTierNamesDefinedInCustomTierSheet(localization);
            _loader = this.GetFixture().Create<IExcelLoader>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new TierFormFieldSheetShouldHaveAllTierNamesDefinedInCustomTierSheet(null);
        }

        private void StubCustomTierSheetData(IExcelLoader loader, params string[] tierNames)
        {
            loader.Stub(x => x.Sheet<CustomTier>())
                  .Return(this.GetFixture().Create<ISheetInfo<CustomTier>>());
            var items = tierNames.Select(x => new CustomTier {TierName = x}).ToList();
            loader.Sheet<CustomTier>().Stub(x => x.Data).Return(items);
        }

        private void StubTierFormFieldSheetData(IExcelLoader loader, params string[] tierNames)
        {
            loader.Stub(x => x.Sheet<TierFormField>())
                  .Return(this.GetFixture().Create<ISheetInfo<TierFormField>>());
            var items = tierNames.Select(x => new TierFormField {TierName = x}).ToList();
            loader.Sheet<TierFormField>().Stub(x => x.Data).Return(items);
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenTierNameNotInCustomTier()
        {
            var orphanTierName = this.GetFixture().Create<string>();
            var customTierNames = this.GetFixture().CreateMany<string>().ToArray();

            StubCustomTierSheetData(_loader, customTierNames);
            StubTierFormFieldSheetData(_loader, customTierNames.Concat(new[] {orphanTierName}).ToArray());

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
            Assert.IsTrue(result.Messages.First().Message.Contains(orphanTierName));
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenTierNamesAreAllInCustomTier()
        {
            var customTierNames = this.GetFixture().CreateMany<string>().ToArray();

            StubCustomTierSheetData(_loader, customTierNames);
            StubTierFormFieldSheetData(_loader, customTierNames.Skip(1).ToArray());

            var result = _sut.Check(_loader, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }
    }
}