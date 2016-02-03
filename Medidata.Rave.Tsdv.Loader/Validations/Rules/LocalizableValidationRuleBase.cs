using System;
using System.Collections.Generic;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
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

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, Action next)
        {
            Validate(blockPlan, out messages, _localization, next);
        }

        protected abstract void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages,
                                         ILocalization localization, Action next);

        protected virtual IValidationMessage CreateErrorMessage(string messageId, params object[] args)
        {
            var message = ComposeLocalizedMessage(messageId, args);
            return message.ToValidationError();
        }

        protected virtual IValidationMessage CreateWarnignMessage(string messageId, params object[] args)
        {
            var message = ComposeLocalizedMessage(messageId, args);
            return message.ToValidationWarning();
        }

        private string ComposeLocalizedMessage(string messageId, params object[] args)
        {
            if (string.IsNullOrEmpty(messageId)) return messageId;
            var localizedString = _localization.GetLocalString(messageId);
            return args == null || args.Length == 0 ? localizedString : string.Format(localizedString, args);
        }
    }
}