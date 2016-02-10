using System;
using System.Linq;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.Tests.TestHelpers;
using Medidata.Rave.Tsdv.Loader.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Ploeh.AutoFixture;

namespace Medidata.Rave.Tsdv.Loader.Tests.Validations
{
    [TestClass]
    public class I18NValidationRuleBaseTests
    {
        private I18NValidationRuleBase _sut;

        [TestInitialize]
        public void Init()
        {
            var localization = TestHelper.CreateStubLocalization();
            _sut = MockRepository.GeneratePartialMock<I18NValidationRuleBase>(localization);
            _sut.Stub(x => x.CreateErrorMessage(null)).IgnoreArguments().CallOriginalMethod(OriginalCallOptions.NoExpectation);
            _sut.Stub(x => x.CreateWarnignMessage(null)).IgnoreArguments().CallOriginalMethod(OriginalCallOptions.NoExpectation);

        }

        [TestMethod]
        public void Ctor_NullLocalization()
        {
            var hasException = false;
            try
            {
                ILocalization localization = null;
                _sut = MockRepository.GeneratePartialMock<I18NValidationRuleBase>(localization);
            }
            catch (Exception e)
            {
                hasException = true;
                Assert.IsInstanceOfType(e.InnerException, typeof(ArgumentNullException));
            }
            Assert.IsTrue(hasException);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateErrorMessage_NullMessageId()
        {
            var result = _sut.CreateErrorMessage(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateErrorMessage_EmptyMessageId()
        {
            var result = _sut.CreateErrorMessage(null);
        }

        [TestMethod]
        public void CreateErrorMessage_WithoutParams()
        {
            var messageId = this.GetFixture().Create<string>();

            var result = _sut.CreateErrorMessage(messageId);

            Assert.IsNotNull(result);
            Assert.AreEqual("[" + messageId + "]", result.Message);
        }

        [TestMethod]
        public void CreateErrorMessage_WithParams()
        {
            var messageId = "{0}-{1}-{2}";
            var args = this.GetFixture().CreateMany<object>().ToArray();

            var result = _sut.CreateErrorMessage(messageId, args);

            Assert.IsNotNull(result);
            var expectedMessage = string.Format("[" + messageId + "]", args);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [TestMethod]
        public void CreateWarningMessage_WithoutParams()
        {
            var messageId = this.GetFixture().Create<string>();

            var result = _sut.CreateWarnignMessage(messageId);

            Assert.IsNotNull(result);
            Assert.AreEqual("[" + messageId + "]", result.Message);
        }

        [TestMethod]
        public void CreateWarnignMessage_WithParams()
        {
            var messageId = "{0}-{1}-{2}";
            var args = this.GetFixture().CreateMany<object>().ToArray();

            var result = _sut.CreateWarnignMessage(messageId, args);

            Assert.IsNotNull(result);
            var expectedMessage = string.Format("[" + messageId + "]", args);
            Assert.AreEqual(expectedMessage, result.Message);
        }
    }
}
