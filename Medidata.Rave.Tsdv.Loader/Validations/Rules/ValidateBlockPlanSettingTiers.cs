using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Interfaces.Localization;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class ValidateBlockPlanSettingTiers : LocalizableValidationRuleBase
    {
        static readonly List<string> DefaultTiers = new List<string> { "Architect Defined", "No Forms", "All Forms" };

        public ValidateBlockPlanSettingTiers(ILocalization localization)
            : base(localization)
        {
            
        }

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, ILocalization localization, Action next)
        {
            messages = new List<IValidationMessage>();

            foreach (var blockTier in blockPlan.Sheet<BlockPlanSetting>().Definition.ExtraColumnDefinitions)
            {
                if (blockPlan.Sheet<CustomTier>().Data.All(x => x.TierName != blockTier.PropertyName) &&
                    DefaultTiers.All(x => x != blockTier.PropertyName))
                {
                    messages.Add(String.Format("'{0} tier header in BlockPlanSettings is not defined in CustomTier.",
                    String.Join(",", blockTier.PropertyName)).ToValidationError());
                }
            }

            if (messages.Count > 0)
            {
                return;
            }

            next();
        }
    }
}