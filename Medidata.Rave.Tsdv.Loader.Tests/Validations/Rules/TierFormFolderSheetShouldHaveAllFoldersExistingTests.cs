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
    public class TierFormFolderSheetShouldHaveAllFoldersExistingTests
    {
        private IValidationHelper _helper;
        private IExcelLoader _loader;
        private TierFormFolderSheetShouldHaveAllFoldersExisting _sut;
        private ILocalization _localization;

        [TestInitialize]
        public void Init()
        {
            _helper = this.GetFixture().Create<IValidationHelper>();
            _localization = TestHelper.CreateStubLocalization();
            _sut = new TierFormFolderSheetShouldHaveAllFoldersExisting(_helper, _localization);
            _loader = this.GetFixture().Create<IExcelLoader>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new TierFormFolderSheetShouldHaveAllFoldersExisting(_helper, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullHelper()
        {
            _sut = new TierFormFolderSheetShouldHaveAllFoldersExisting(null, _localization);
        }

        private void StubTierFormFolderSheetData(IExcelLoader loader, params string[] folderOids)
        {
            loader.Stub(x => x.Sheet<TierFormFolder>())
                  .Return(this.GetFixture().Create<ISheetInfo<TierFormFolder>>());
            loader.Sheet<TierFormFolder>().Stub(x => x.Definition).Return(this.GetFixture().Create<ISheetDefinition>());
            var colDefs = folderOids.Select(x => new ColumnDefinition { PropertyName = x, ExtraProperty = true });
            loader.Sheet<TierFormFolder>().Definition.Stub(x => x.ColumnDefinitions).Return(colDefs);
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenFolderOidNotExisting()
        {
            var folderOid = this.GetFixture().Create<string>();
            var context = this.GetFixture().Create<IDictionary<string, object>>();

            _helper.Stub(x => x.ExistsFolderOid(folderOid,context)).Return(false);
            StubTierFormFolderSheetData(_loader, folderOid);

            var result = _sut.Check(_loader, context);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ShouldContinue);
            Assert.IsTrue(result.Messages.Any());
            Assert.IsTrue(result.Messages.First().Message.Contains(folderOid));
        }

        [TestMethod]
        public void Validate_ShouldNotPass_WhenFolderOidIsExisting()
        {
            var folderOid = this.GetFixture().Create<string>();
            var context = this.GetFixture().Create<IDictionary<string, object>>();

            _helper.Stub(x => x.ExistsFolderOid(folderOid, context)).Return(true);
            StubTierFormFolderSheetData(_loader, folderOid);

            var result = _sut.Check(_loader, context);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ShouldContinue);
            Assert.IsFalse(result.Messages.Any());
        }
    }
}