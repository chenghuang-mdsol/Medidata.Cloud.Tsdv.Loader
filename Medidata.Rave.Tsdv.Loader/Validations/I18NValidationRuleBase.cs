using System;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Cloud.ExcelLoader.Validations.Rules;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.Validations
{
    public abstract class I18NValidationRuleBase : ValidationRuleBase
    {
        protected I18NValidationRuleBase(ILocalization localization)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            Localization = localization;
        }

        public ILocalization Localization { get; private set; }

        public virtual IValidationMessage CreateErrorMessage(string messageId, params object[] args)
        {
            var message = ComposeLocalizedMessage(messageId, args);
            return message.ToValidationError();
        }

        public virtual IValidationMessage CreateWarnignMessage(string messageId, params object[] args)
        {
            var message = ComposeLocalizedMessage(messageId, args);
            return message.ToValidationWarning();
        }

        private string ComposeLocalizedMessage(string messageId, params object[] args)
        {
            if (string.IsNullOrEmpty(messageId)) throw new ArgumentException("messageId");
            var localizedString = Localization.GetLocalString(messageId);
            return args == null || args.Length == 0 ? localizedString : string.Format(localizedString, args);
        }
    }
}