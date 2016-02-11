using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.DefinedNamedRange;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests.SheetDefinitions.Presentation
{
    [TestClass]
    public class TsdvPresentationLoaderFactoryTests
    {
        private IFixture _fixture;
        private TsdvPresentationLoaderFactory _sut;
        private IExcelLoader _loader;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());

            var localization = _fixture.Create<ILocalization>();
            var namedRangeProvider = _fixture.Create<INamedRangeProvider>();
            _sut = MockRepository.GeneratePartialMock<TsdvPresentationLoaderFactory>(localization, namedRangeProvider);

            _loader = _fixture.Create<IExcelLoader>();
            StubSheet<BlockPlan>(_loader);
            StubSheet<BlockPlanSetting>(_loader);
            StubSheet<CustomTier>(_loader);
            StubSheet<TierForm>(_loader);
            StubSheet<TierField>(_loader);
            StubSheet<TierFolder>(_loader);
            StubSheet<ExcludedStatus>(_loader);
            StubSheet<Rule>(_loader);
            _sut.Stub(x => x.CreateTsdvExcelLoader()).Return(_loader);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullLocalization()
        {
            _sut = new TsdvPresentationLoaderFactory(null);
        }

        private void StubSheet<T>(IExcelLoader loader) where T: SheetModel
        {
            var sheetDefinition = _fixture.Create<ISheetDefinition>();
            var sheetInfo = _fixture.Create<ISheetInfo<T>>();
            sheetInfo.Stub(x => x.Definition).Return(sheetDefinition);
            loader.Stub(x => x.Sheet<T>()).Return(sheetInfo);
        }

        [TestMethod]
        public void CurrectVersionShouldLoader()
        {
            var version = TsdvLoaderSupportedVersion.Presentation;

            var result = _sut.Create(version);

            _loader.AssertWasCalled(x => x.Sheet<BlockPlan>());
            _loader.AssertWasCalled(x => x.Sheet<BlockPlanSetting>());
            _loader.AssertWasCalled(x => x.Sheet<CustomTier>());
            _loader.AssertWasCalled(x => x.Sheet<TierForm>());
            _loader.AssertWasCalled(x => x.Sheet<TierField>());
            _loader.AssertWasCalled(x => x.Sheet<TierFolder>());
            _loader.AssertWasCalled(x => x.Sheet<ExcludedStatus>());
            _loader.AssertWasCalled(x => x.Sheet<Rule>());
            Assert.AreSame(_loader, result);
        }

        [TestMethod]
        public void IncorrectVersionShouldCallBase()
        {
            var version = TsdvLoaderSupportedVersion.V1;
            Exception ex = null;
            try
            {
                _sut.Create(version);
            }
            catch (Exception e)
            {
                ex = e;
            }

            _loader.AssertWasNotCalled(x => x.Sheet<BlockPlan>());
            _loader.AssertWasNotCalled(x => x.Sheet<BlockPlanSetting>());
            _loader.AssertWasNotCalled(x => x.Sheet<CustomTier>());
            _loader.AssertWasNotCalled(x => x.Sheet<TierForm>());
            _loader.AssertWasNotCalled(x => x.Sheet<TierField>());
            _loader.AssertWasNotCalled(x => x.Sheet<TierFolder>());
            _loader.AssertWasNotCalled(x => x.Sheet<ExcludedStatus>());
            _loader.AssertWasNotCalled(x => x.Sheet<Rule>());
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotSupportedException));
        }
    }
}