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
    public class ValidateTierFields : LocalizableValidationRuleBase
    {
        private readonly Func<string, bool> _validFormOidFunc;
        private readonly Func<string, string, bool> _validFormFieldFunc;

        public ValidateTierFields(ILocalization localization, Func<string, bool> validFormOidFunc, Func<string, string, bool> validFormFieldFunc)
            : base(localization)
        {
            _validFormOidFunc = validFormOidFunc;
            _validFormFieldFunc = validFormFieldFunc;
        }

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, ILocalization localization, Action next)
        {
            messages = new List<IValidationMessage>();

            foreach (var tierField in blockPlan.Sheet<TierField>().Data)
            {
                if (blockPlan.Sheet<CustomTier>().Data.All(x => x.TierName != tierField.TierName))
                {
                    messages.Add(String.Format("'{0} tier name in TierFields is not defined in CustomTier.", tierField.TierName).ToValidationError());
                }

                if (!_validFormOidFunc(tierField.FormOid))
                {
                    messages.Add(String.Format("{0} is not a valid Form OID for the project.", tierField.FormOid).ToValidationError());
                }
                else if (!_validFormFieldFunc(tierField.FormOid, tierField.Fields))
                {
                    messages.Add(
                        String.Format("{0} is not a valid field in form {1}.", tierField.Fields, tierField.FormOid)
                            .ToValidationError());
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