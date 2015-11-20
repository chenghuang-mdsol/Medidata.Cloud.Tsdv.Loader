using System;
using Medidata.Interfaces.TSDV;

namespace Medidata.Rave.Tsdv.Loader.Validation.Rules
{
    public abstract class ValidationRuleBase : IValidationRule
    {
        public virtual IValidationRuleResult Check(IBlockPlan blockPlan)
        {
            var shouldContinue = true;
            Action next = () => { shouldContinue = true; };
            IValidationMessage message;
            Validate(blockPlan, out message, next);
            return new ValidationRuleResult {Message = message, ShouldContinue = shouldContinue};
        }

        protected abstract void Validate(IBlockPlan blockPlan, out IValidationMessage message, Action next);
    }
}