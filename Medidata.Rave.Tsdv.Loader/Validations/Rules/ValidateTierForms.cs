using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
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
                    messages.Add(String.Format("'{0} tier in TierForms is not defined in CustomTier.", tierForm.TierName).ToValidationError());
                }

                if (!_validFormOidFunc(tierForm.FormOid))
                {
                    messages.Add(String.Format("{0} is not a valid Form OID for the project.", tierForm.Forms).ToValidationError());
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