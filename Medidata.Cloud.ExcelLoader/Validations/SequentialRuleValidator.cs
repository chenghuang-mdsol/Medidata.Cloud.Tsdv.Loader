using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public class SequentialRuleValidator : IValidator
    {
        private readonly bool _earlyExit;
        private readonly IEnumerable<IValidationRule> _rules;

        public SequentialRuleValidator(bool earlyExit, params IValidationRule[] rules)
        {
            _earlyExit = earlyExit;
            _rules = rules ?? Enumerable.Empty<IValidationRule>();
        }

        public IValidationResult Validate(IExcelLoader excelLoader, IDictionary<string, object> context)
        {
            if (excelLoader == null) throw new ArgumentNullException("excelLoader");

            var messages = new List<IValidationMessage>();
            var result = new ValidationResult {ValidationTarget = excelLoader, Messages = messages };
            var contextDic = context ?? new Dictionary<string, object>();

            try
            {
                foreach (var ruleResult in _rules.Select(r => r.Check(excelLoader, contextDic)))
                {
                    messages.AddRange(ruleResult.Messages);
                    if (_earlyExit && !ruleResult.ShouldContinue)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                var error = e.ToString().ToValidationError();
                messages.Add(error);
            }

            return result;
        }
    }
}