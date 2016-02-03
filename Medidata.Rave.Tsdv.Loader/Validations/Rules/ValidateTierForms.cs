using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.Helpers;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class ValidateTierForms : LocalizableValidationRuleBase
    {
        private readonly Func<string, bool> _validFormOidFunc;
 
        public ValidateTierForms(ILocalization localization, Func<string, bool> validFormOidFunc) : base(localization)
        {
            _validFormOidFunc = validFormOidFunc;
        }

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, ILocalization localization, Action next)
        {
            messages = new List<IValidationMessage>();

            foreach (var tierForm in blockPlan.Sheet<TierForm>().Data)
            {
                if (blockPlan.Sheet<CustomTier>().Data.All(x => x.TierName != tierForm.TierName))
                {
                    var message = CreateErrorMessage("'{0} tier in TierForms is not defined in CustomTier.", tierForm.TierName);
                    messages.Add(message);
                }

                if (!_validFormOidFunc(tierForm.FormOid))
                {
                    var message = CreateErrorMessage("{0} is not a valid Form OID for the project.", tierForm.Forms);
                    messages.Add(message);
                }
            }

            if (messages.Any())
            {
                return;
            }

            next();
        }
    }
}