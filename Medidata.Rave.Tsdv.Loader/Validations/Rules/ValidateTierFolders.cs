using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class ValidateTierFolders : LocalizableValidationRuleBase
    {
        private readonly Func<string, bool> _validFormFolderFunc;

        public ValidateTierFolders(ILocalization localization, Func<string, bool> validFormFolderFunc) : base(localization)
        {
            _validFormFolderFunc = validFormFolderFunc;
        }

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, ILocalization localization, Action next)
        {
            messages = new List<IValidationMessage>();

            foreach (var tierFolder in blockPlan.Sheet<TierFolder>().Definition.ExtraColumnDefinitions)
            {
                if (!_validFormFolderFunc(tierFolder.PropertyName))
                {
                    var message = CreateErrorMessage("'{0} folder header in TierFolders is not valid.", tierFolder.PropertyName);
                    messages.Add(message);
                }
            }

            if (messages.Any())
            {
                return;
            }

            foreach (var tierForm in blockPlan.Sheet<TierFolder>().Data)
            {
                if (blockPlan.Sheet<TierForm>().Data.All(x => x.TierName != tierForm.TierName && x.FormOid != tierForm.FormOid))
                {
                    var message = CreateErrorMessage("The Form OID {0} has not been selected for the tier {1} in TierForms.", tierForm.FormOid, tierForm.TierName);
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