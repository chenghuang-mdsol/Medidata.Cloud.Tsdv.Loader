using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Interfaces.TSDV;
using Medidata.Rave.Tsdv.Loader.Helpers;

namespace Medidata.Rave.Tsdv.Loader.Validation
{
    public class DefaultSequentialRuleValidator : IValidator
    {
        private readonly IEnumerable<IValidationRule> _rules;

        public DefaultSequentialRuleValidator(params IValidationRule[] rules)
        {
            _rules = rules ?? Enumerable.Empty<IValidationRule>();
        }

        public IValidationResult Validate(IBlockPlan blockPlan)
        {
            if (blockPlan == null) throw new ArgumentNullException("blockPlan");

            var result = new ValidationResult {ValidationTarget = blockPlan};

            try
            {
                foreach (var ruleResult in _rules.Select(r => r.Check(blockPlan)))
                {
                    result.Messages.Add(ruleResult.Message);
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