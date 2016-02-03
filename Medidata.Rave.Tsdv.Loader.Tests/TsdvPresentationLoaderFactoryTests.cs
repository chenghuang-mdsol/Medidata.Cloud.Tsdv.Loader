using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests
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
            _sut = MockRepository.GeneratePartialMock<TsdvPresentationLoaderFactory>(localization);

            _loader = _fixture.Create<IExcelLoader>();
            _sut.Stub(x => x.CreateTsdvExcelLoader()).Return(_loader);
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