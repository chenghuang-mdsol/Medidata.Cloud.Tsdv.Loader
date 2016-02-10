using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Medidata.Cloud.ExcelLoader.Validations;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.ExcelLoader.Tests.TestHelpers
{
    public static class TestHelper
    {
        private static readonly IFixture Fixture = new Fixture().Customize(new AutoRhinoMockCustomization());

        public static IValidationRule NewStubValidationRule<T>(bool canContinue, T message, int delayMillionseconds = 0) where T : IValidationMessage
        {
            var ruleResult = Fixture.Create<IValidationRuleResult>();
            ruleResult.Stub(x => x.ShouldContinue).Return(canContinue);
            ruleResult.Stub(x => x.Messages).Return(new List<IValidationMessage> { message });

            var rule = Fixture.Create<IValidationRule>();
            rule.Stub(x => x.Check(null, null)).IgnoreArguments().Return(null).WhenCalled(a =>
            {
                if (delayMillionseconds > 0)
                {
                    Thread.Sleep(delayMillionseconds);
                }
                a.ReturnValue = ruleResult;
            });

            return rule;
        }

        public static IValidationRule NewStubValidationRule(Exception exception)
        {
            var rule = Fixture.Create<IValidationRule>();
            rule.Stub(x => x.Check(null, null)).IgnoreArguments().Throw(exception);
            return rule;
        }

        public static IEnumerable<IValidationMessage> CreateManyValidationMessages()
        {
            var messages = Fixture.CreateMany<IValidationMessage>().ToList();
            messages.ForEach(x => x.Stub(y => y.Message).Return(Fixture.Create<string>()));
            return messages;
        } 
    }
}