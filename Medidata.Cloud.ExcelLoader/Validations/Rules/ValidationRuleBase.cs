using System;
using System.Collections.Generic;
using System.Linq;

namespace Medidata.Cloud.ExcelLoader.Validations.Rules
{
    public abstract class ValidationRuleBase : IValidationRule
    {
        public virtual IValidationRuleResult Check(IExcelLoader excelLoader, IDictionary<string, object> context)
        {
            bool shouldContinue;
            var messages = Validate(excelLoader, context, out shouldContinue);
            return new ValidationRuleResult {Messages = messages.ToList(), ShouldContinue = shouldContinue};
        }

        protected internal abstract IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader, IDictionary<string, object> context, out bool shouldContinue);
    }
}