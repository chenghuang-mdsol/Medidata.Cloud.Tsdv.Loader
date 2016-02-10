using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests.SheetDefinitions.v1
{
    [TestClass]
    public class TsdvLoaderFactoryTests
    {
        private IFixture _fixture;
        private TsdvLoaderFactory _sut;
        private IExcelLoader _loader;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());

            var localization = _fixture.Create<ILocalization>();
            _sut = MockRepository.GeneratePartialMock<TsdvLoaderFactory>(localization);

            _loader = _fixture.Create<IExcelLoader>();
            StubSheet<BlockPlanSetting>(_loader);
            StubSheet<CustomTier>(_loader);
            StubSheet<TierFormField>(_loader);
            StubSheet<TierFormFolder>(_loader);
            StubSheet<Rule>(_loader);

            _sut.Stub(x => x.CreateTsdvExcelLoader()).Return(_loader);
        }

        private void StubSheet<T>(IExcelLoader loader) where T : SheetModel
        {
            var sheetDefinition = _fixture.Create<ISheetDefinition>();
            var sheetInfo = _fixture.Create<ISheetInfo<T>>();
            sheetInfo.Stub(x => x.Definition).Return(sheetDefinition);
            loader.Stub(x => x.Sheet<T>()).Return(sheetInfo);
        }

        [TestMethod]
        public void CurrectVersionShouldLoader()
        {
            var version = TsdvLoaderSupportedVersion.V1;

            var result = _sut.Create(version);

            _loader.AssertWasCalled(x => x.Sheet<BlockPlanSetting>());
            _loader.AssertWasCalled(x => x.Sheet<CustomTier>());
            _loader.AssertWasCalled(x => x.Sheet<TierFormField>());
            _loader.AssertWasCalled(x => x.Sheet<TierFormFolder>());
            _loader.AssertWasCalled(x => x.Sheet<Rule>());
            Assert.AreSame(_loader, result);
        }

        [TestMethod]
        public void IncorrectVersionShouldCallBase()
        {
            var version = TsdvLoaderSupportedVersion.Presentation;
            Exception ex = null;
            try
            {
                _sut.Create(version);
            }
            catch
            {
            }

            _loader.AssertWasNotCalled(x => x.Sheet<BlockPlanSetting>());
            _loader.AssertWasNotCalled(x => x.Sheet<CustomTier>());
            _loader.AssertWasNotCalled(x => x.Sheet<TierFormField>());
            _loader.AssertWasNotCalled(x => x.Sheet<TierFormFolder>());
            _loader.AssertWasNotCalled(x => x.Sheet<Rule>());
        }
    }
}