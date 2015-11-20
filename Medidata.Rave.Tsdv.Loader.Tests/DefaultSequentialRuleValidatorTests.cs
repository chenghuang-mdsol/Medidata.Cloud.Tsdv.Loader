using System.Linq;
using Medidata.Interfaces.TSDV;
using Medidata.Rave.Tsdv.Loader.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests
{
    [TestClass]
    public class DefaultSequentialRuleValidatorTests
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
            var blockPlan = _fixture.Create<IBlockPlan>();
            var messages = _fixture.CreateMany<IValidationError>().ToArray();
            var validationRules = new[]
            {
                CreateValidationRule(true, messages[0]),
                CreateValidationRule(true, messages[1]),
                CreateValidationRule(true, messages[2])
            };

            // Act
            var sut = new DefaultSequentialRuleValidator(validationRules);
            var result = sut.Validate(blockPlan);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(blockPlan, result.ValidationTarget);
            CollectionAssert.AreEquivalent(messages, result.Messages.ToArray());
            Assert.IsFalse(result.Messages.OfType<IValidationWarning>().Any());
        }

        [TestMethod]
        public void StopRuleIfShouldContinueIsFalse()
        {
            // Arrange
            var blockPlan = _fixture.Create<IBlockPlan>();
            var messages = _fixture.CreateMany<IValidationMessage>().ToArray();
            var validationRules = new[]
            {
                CreateValidationRule(true, messages[0]),
                CreateValidationRule(false, messages[1]),
                CreateValidationRule(true, messages[2])
            };

            // Act
            var sut = new DefaultSequentialRuleValidator(validationRules);
            var result = sut.Validate(blockPlan);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(blockPlan, result.ValidationTarget);
            CollectionAssert.AreEquivalent(messages.Take(2).ToArray(), result.Messages.ToArray());
        }

        private IValidationRule CreateValidationRule<T>(bool canContinue, T message)
            where T : IValidationMessage
        {
            var ruleResult = _fixture.Create<IValidationRuleResult>();
            ruleResult.Stub(x => x.ShouldContinue).Return(canContinue);
            ruleResult.Stub(x => x.Message).Return(message);

            var rule = _fixture.Create<IValidationRule>();
            rule.Stub(x => x.Check(null)).IgnoreArguments().Return(ruleResult);

            return rule;
        }
    }
}