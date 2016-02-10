using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class BlockPlanSettingSheetShouldHaveCustomTierNamesDefinedInCustomTierSheet : I18NValidationRuleBase
    {
        private static readonly string[] DefaultTiers = {"Architect Defined", "No Forms", "All Forms"};

        public BlockPlanSettingSheetShouldHaveCustomTierNamesDefinedInCustomTierSheet(ILocalization localization)
            : base(localization) {}

        protected override IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader,
                                                                    IDictionary<string, object> context,
                                                                    out bool shouldContinue)
        {
            var tierNamesInBlockPlanSettings = excelLoader.Sheet<BlockPlanSetting>()
                                                          .Definition
                                                          .ColumnDefinitions
                                                          .Select(x => x.PropertyName);
            var tierNamesInCustomTiers = excelLoader.Sheet<CustomTier>()
                                                    .Data
                                                    .Select(x => x.TierName);
            var badTierNames = tierNamesInBlockPlanSettings.Except(DefaultTiers)
                                                           .Except(tierNamesInCustomTiers);
            var messages = badTierNames.Select(CreateErrorMessage).ToArray();
            shouldContinue = messages.Length == 0;
            return messages;
        }

        private IValidationMessage CreateErrorMessage(string tierName)
        {
            return CreateErrorMessage("'{0}' tier header in BlockPlanSettings is not defined in CustomTier.",
                tierName);
        }
    }
}