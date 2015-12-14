using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Cloud.ExcelLoader.Validations.Rules;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public abstract class LocalizableValidationRuleBase : ValidationRuleBase
    {
        private readonly ILocalization _localization;

        protected LocalizableValidationRuleBase(ILocalization localization)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        protected override void Validate(IExcelParser blockPlan, out IValidationMessage message, Action next)
        {
            Validate(blockPlan, out message, _localization, next);
        }

        protected abstract void Validate(IExcelParser blockPlan, out IValidationMessage message,
            ILocalization localization, Action next);
    }
}