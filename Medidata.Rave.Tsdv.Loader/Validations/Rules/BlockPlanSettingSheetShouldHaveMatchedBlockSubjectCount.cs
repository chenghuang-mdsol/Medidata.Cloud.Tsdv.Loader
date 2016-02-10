using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class BlockPlanSettingSheetShouldHaveMatchedBlockSubjectCount : I18NValidationRuleBase
    {
        public BlockPlanSettingSheetShouldHaveMatchedBlockSubjectCount(ILocalization localization) : base(localization) {}

        protected override IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader,
                                                                    IDictionary<string, object> context,
                                                                    out bool shouldContinue)
        {
            var messages = new List<IValidationMessage>();
            foreach (var blockPlanSetting in excelLoader.Sheet<BlockPlanSetting>().Data)
            {
                if (blockPlanSetting.BlockSubjectCount == 0)
                {
                    var message = CreateErrorMessage("tsdv_BlockSizeZeroError", blockPlanSetting.Block);
                    messages.Add(message);
                }
                else if (TotalTierCountMismatched(blockPlanSetting))
                {
                    var message = CreateErrorMessage("tsdv_BlockValidationError", blockPlanSetting.Block);
                    messages.Add(message);
                }
            }

            shouldContinue = messages.Count == 0;
            return messages;
        }

        private bool TotalTierCountMismatched(BlockPlanSetting block)
        {
            var totalTierSubjectCount = block.GetExtraProperties().Values.OfType<int>().Sum();
            return totalTierSubjectCount != block.BlockSubjectCount;
        }
    }
}