using System;
using Medidata.Interfaces.Localization;
using Medidata.Interfaces.TSDV;

namespace Medidata.Rave.Tsdv.Loader.Validation.Rules
{
    public abstract class LocalizableValidationRuleBase : ValidationRuleBase
    {
        private readonly ILocalization _localization;

        protected LocalizableValidationRuleBase(ILocalization localization)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        protected override void Validate(IBlockPlan blockPlan, out IValidationMessage message, Action next)
        {
            Validate(blockPlan, out message, _localization, next);
        }

        protected abstract void Validate(IBlockPlan blockPlan, out IValidationMessage message,
            ILocalization localization, Action next);
    }
}