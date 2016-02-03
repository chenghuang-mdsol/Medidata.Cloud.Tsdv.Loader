using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public class DefaultSequentialRuleValidator : IValidator
    {
        private readonly IEnumerable<IValidationRule> _rules;

        public DefaultSequentialRuleValidator(params IValidationRule[] rules)
        {
            _rules = rules ?? Enumerable.Empty<IValidationRule>();
        }

        public IValidationResult Validate(IExcelLoader excelLoader)
        {
            if (excelLoader == null) throw new ArgumentNullException("excelParser");

            var result = new ValidationResult {ValidationTarget = excelLoader};

            try
            {
                foreach (var ruleResult in _rules.Select(r => r.Check(excelLoader)))
                {
                    result.Messages.AddRange(ruleResult.Messages);
                    if (!ruleResult.ShouldContinue)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                var error = e.ToString().ToValidationError();
                result.Messages.Add(error);
            }

            return result;
        }
    }
}