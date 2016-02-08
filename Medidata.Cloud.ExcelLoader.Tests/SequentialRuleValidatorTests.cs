using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Tests.TestHelpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.ExcelLoader.Tests
{
    [TestClass]
    public class SequentialRuleValidatorTests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
        }

        [TestMethod]
        public void AllRulesAreChecked()
        {
            // Arrange
            var loader = _fixture.Create<IExcelLoader>();
            var context = _fixture.Create<IDictionary<string, object>>();
            var messages = TestHelper.CreateManyValidationMessages().ToArray();
            var validationRules = new[]
                                  {
                                      TestHelper.NewStubValidationRule(true, messages[0]),
                                      TestHelper.NewStubValidationRule(true, messages[1]),
                                      TestHelper.NewStubValidationRule(true, messages[2])
                                  };

            // Act
            var sut = new SequentialRuleValidator(false, validationRules);
            var result = sut.Validate(loader, context);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(loader, result.ValidationTarget);
            CollectionAssert.AreEquivalent(messages, result.Messages.ToArray());
            Assert.IsFalse(result.Messages.OfType<IValidationWarning>().Any());
        }

        [TestMethod]
        public void StopRuleIfEarlyExistIsTrueAndShouldContinueIsFalse()
        {
            // Arrange
            var loader = _fixture.Create<IExcelLoader>();
            var context = _fixture.Create<IDictionary<string, object>>();
            var messages = TestHelper.CreateManyValidationMessages().ToArray();
            var validationRules = new[]
                                  {
                                      TestHelper.NewStubValidationRule(true, messages[0]),
                                      TestHelper.NewStubValidationRule(false, messages[1]),
                                      TestHelper.NewStubValidationRule(true, messages[2])
                                  };

            // Act
            var sut = new SequentialRuleValidator(true, validationRules);
            var result = sut.Validate(loader, context);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(loader, result.ValidationTarget);
            CollectionAssert.AreEquivalent(messages.Take(2).ToArray(), result.Messages.ToArray());
        }

        [TestMethod]
        public void AllRuleIfEarlyExistIsFalseAndShouldContinueIsFalse()
        {
            // Arrange
            var loader = _fixture.Create<IExcelLoader>();
            var context = _fixture.Create<IDictionary<string, object>>();
            var messages = TestHelper.CreateManyValidationMessages().ToArray();
            var validationRules = new[]
                                  {
                                      TestHelper.NewStubValidationRule(true, messages[0]),
                                      TestHelper.NewStubValidationRule(false, messages[1]),
                                      TestHelper.NewStubValidationRule(true, messages[2])
                                  };

            // Act
            var sut = new SequentialRuleValidator(false, validationRules);
            var result = sut.Validate(loader, context);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(loader, result.ValidationTarget);
            CollectionAssert.AreEquivalent(messages, result.Messages.ToArray());
            Assert.IsFalse(result.Messages.OfType<IValidationWarning>().Any());
        }

        [TestMethod]
        public void ExceptionWillTerminateValidation()
        {
            // Arrange
            var loader = _fixture.Create<IExcelLoader>();
            var context = _fixture.Create<IDictionary<string, object>>();
            var exception = _fixture.Create<Exception>();
            var messages = TestHelper.CreateManyValidationMessages().ToArray();
            var validationRules = new[]
                                  {
                                      TestHelper.NewStubValidationRule(true, messages[0]),
                                      TestHelper.NewStubValidationRule(exception),
                                      TestHelper.NewStubValidationRule(true, messages[2])
                                  };

            // Act
            var sut = new SequentialRuleValidator(false, validationRules);
            var result = sut.Validate(loader, context);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(loader, result.ValidationTarget);
            var expectedMessages = new[] {messages[0], exception.ToString().ToValidationError()}.Select(x => x.Message).ToArray();
            var actualMessages = result.Messages.Select(x => x.Message).ToArray();
            CollectionAssert.AreEquivalent(expectedMessages, actualMessages);
            Assert.IsFalse(result.Messages.OfType<IValidationWarning>().Any());
        }
    }
}