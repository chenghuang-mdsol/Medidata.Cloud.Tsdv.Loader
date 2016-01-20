using System;
using System.Collections.Generic;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class ValdiateBlockPlans : LocalizableValidationRuleBase
    {
        private readonly Func<string, bool> _validDataEntryRole;
 
        public ValdiateBlockPlans(ILocalization localization, Func<string, bool> validDataEntryRole) : base(localization)
        {
            _validDataEntryRole = validDataEntryRole;
        }

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, ILocalization localization, Action next)
        {
            messages = new List<IValidationMessage>();

            if (blockPlan.Sheet<BlockPlan>().Data.Count < 1)
            {
                messages.Add(String.Format("No block plan to load.").ToValidationError());
                return;
            }

            if (blockPlan.Sheet<BlockPlan>().Data.Count > 1)
            {
                messages.Add(String.Format("Only one block plan can be loaded.").ToValidationError());
                return;
            }

            var role = blockPlan.Sheet<BlockPlan>().Data[0].DataEntryRole;
            if (!_validDataEntryRole(role))
            {
                messages.Add(String.Format("Data Entry {0} is not valid.", role).ToValidationError());
                return;
            }

            next();
        }
    }
}