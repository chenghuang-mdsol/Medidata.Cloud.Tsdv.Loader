using System;
using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations.Rules
{
    public abstract class ValidationRuleBase : IValidationRule
    {
        public virtual IValidationRuleResult Check(IExcelLoader excelLoader, IDictionary<string, object> context)
        {
            var shouldContinue = false;
            Action next = () => { shouldContinue = true; };
            IList<IValidationMessage> messages;
            Validate(excelLoader, context, out messages, next);
            return new ValidationRuleResult {Messages = messages, ShouldContinue = shouldContinue};
        }

        internal abstract void Validate(IExcelLoader excelLoader, IDictionary<string, object> context, out IList<IValidationMessage> message, Action next);
    }
}