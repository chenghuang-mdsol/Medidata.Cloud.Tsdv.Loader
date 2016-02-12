using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class BlockPlanSettingSheetShouldHaveMoreThanZeroTiers : I18NValidationRuleBase
    {
        public BlockPlanSettingSheetShouldHaveMoreThanZeroTiers(ILocalization localization) : base(localization) {}

        protected override IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader,
                                                                    IDictionary<string, object> context,
                                                                    out bool shouldContinue)
        {
            var messages = (from bps in excelLoader.Sheet<BlockPlanSetting>().Data
                            let totalTierCount = bps.GetExtraProperties().Values.OfType<int>().Sum()
                            where totalTierCount == 0
                            select CreateErrorMessage("tsdv_BlockValidationError", bps.Block))
                            .ToList();

            shouldContinue = messages.Count == 0;
            return messages;
        }
    }
}