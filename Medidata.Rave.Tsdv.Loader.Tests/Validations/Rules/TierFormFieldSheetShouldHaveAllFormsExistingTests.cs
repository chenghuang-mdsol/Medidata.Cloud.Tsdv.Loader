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
    public class TierFormFieldSheetShouldHaveAllFormsExistingTests
    {
        private IValidationHelper _helper;
        private IExcelLoader _loader;
        private TierFormFieldSheetShouldHaveAllFormsExisting _sut;
        private ILocalization _localization;

        [TestInitialize]
        public void Init()
        {
            _helper = this.GetFixture().Create<IValidationHelper>();
            _localization = TestHelper.CreateStubLocalization();
            _sut = new TierFormFieldSheetShouldHaveAllFormsExisting(_helper, _localization);
            _loader = this.GetFixture().Create<IExcelLoader>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new TierFormFieldSheetShouldHaveAllFormsExisting(_helper, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullHelper()
        {
            _sut = new TierFormFieldSheetShouldHaveAllFormsExisting(null, _localization);
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenFormOidNotExisting()
        {
            var formOid = this.GetFixture().Create<string>();
            var context = this.GetFixture().Create<IDictionary<string, object>>();

            _helper.Stub(x => x.ExistsFormOid(formOid, context)).Return(false);
            _loader.Stub(x => x.Sheet<TierFormField>())
                   .Return(this.GetFixture().Create<ISheetInfo<TierFormField>>());

            var items = new[]
                        {
                            new TierFormField
                            {
                                FormOid = formOid
                            }
                        };
            _loader.Sheet<TierFormField>().Stub(x => x.Data).Return(items);

            var result = _sut.Check(_loader, context);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
            Assert.IsTrue(result.Messages.First().Message.Contains(formOid));
        }

        [TestMethod]
        public void Validate_ShouldPass_WhenFormOidIsExisting()
        {
            var formOid = this.GetFixture().Create<string>();
            var context = this.GetFixture().Create<IDictionary<string, object>>();

            _helper.Stub(x => x.ExistsFormOid(formOid, context)).Return(true);
            _loader.Stub(x => x.Sheet<TierFormField>())
                   .Return(this.GetFixture().Create<ISheetInfo<TierFormField>>());

            var items = new[]
                        {
                            new TierFormField
                            {
                                FormOid = formOid
                            }
                        };
            _loader.Sheet<TierFormField>().Stub(x => x.Data).Return(items);

            var result = _sut.Check(_loader, context);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }
    }
}