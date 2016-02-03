using System;
using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations.Rules
{
    public abstract class ValidationRuleBase : IValidationRule
    {
        public virtual IValidationRuleResult Check(IExcelLoader excelLoader)
        {
            var shouldContinue = false;
            Action next = () => { shouldContinue = true; };
            IList<IValidationMessage> messages;
            Validate(excelLoader, out messages, next);
            return new ValidationRuleResult {Messages = messages, ShouldContinue = shouldContinue};
        }

        protected abstract void Validate(IExcelLoader excelLoader, out IList<IValidationMessage> message, Action next);
    }
}