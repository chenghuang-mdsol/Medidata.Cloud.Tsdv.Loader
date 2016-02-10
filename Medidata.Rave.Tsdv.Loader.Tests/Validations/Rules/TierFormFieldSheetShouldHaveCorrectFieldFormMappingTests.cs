using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;
using Medidata.Rave.Tsdv.Loader.Tests.TestHelpers;
using Medidata.Rave.Tsdv.Loader.Validations;
using Medidata.Rave.Tsdv.Loader.Validations.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests.Validations.Rules
{
    [TestClass]
    public class TierFormFieldSheetShouldHaveCorrectFieldFormMappingTests
    {
        private IValidationHelper _helper;
        private IExcelLoader _loader;
        private TierFormFieldSheetShouldHaveCorrectFieldFormMapping _sut;
        private ILocalization _localization;

        [TestInitialize]
        public void Init()
        {
            _helper = this.GetFixture().Create<IValidationHelper>();
            _localization = TestHelper.CreateStubLocalization();
            _sut = new TierFormFieldSheetShouldHaveCorrectFieldFormMapping(_helper, _localization);
            _loader = this.GetFixture().Create<IExcelLoader>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new TierFormFieldSheetShouldHaveCorrectFieldFormMapping(_helper, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullHelper()
        {
            _sut = new TierFormFieldSheetShouldHaveCorrectFieldFormMapping(null, _localization);
        }

        private void StubTierFormFieldSheetData(IExcelLoader loader, string formOid, string fieldOid)
        {
            loader.Stub(x => x.Sheet<TierFormField>())
                  .Return(this.GetFixture().Create<ISheetInfo<TierFormField>>());
            var items = new[] { new TierFormField { FormOid = formOid, FieldOid = fieldOid } }.ToList();
            loader.Sheet<TierFormField>().Stub(x => x.Data).Return(items);
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenFormOidAndFieldOidNotExisting()
        {
            var formOid = this.GetFixture().Create<string>();
            var fieldOid = this.GetFixture().Create<string>();
            var context = this.GetFixture().Create<IDictionary<string, object>>();

            _helper.Stub(x => x.ExistsFormField(formOid, fieldOid, context)).Return(false);
            StubTierFormFieldSheetData(_loader, formOid, fieldOid);

            var result = _sut.Check(_loader, context);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
            Assert.IsTrue(result.Messages.First().Message.Contains(formOid));
            Assert.IsTrue(result.Messages.First().Message.Contains(fieldOid));
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenFormOidAndFieldOidIsExisting()
        {
            var formOid = this.GetFixture().Create<string>();
            var fieldOid = this.GetFixture().Create<string>();
            var context = this.GetFixture().Create<IDictionary<string, object>>();

            _helper.Stub(x => x.ExistsFormField(formOid, fieldOid, context)).Return(true);
            StubTierFormFieldSheetData(_loader, formOid, fieldOid);

            var result = _sut.Check(_loader, context);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }
    }
}
