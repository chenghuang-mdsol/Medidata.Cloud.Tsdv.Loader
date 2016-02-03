using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    class ValidateBlockPlanSettings : LocalizableValidationRuleBase
    {
        public ValidateBlockPlanSettings(ILocalization localization) : base(localization)
        {
        }

        protected override void Validate(IExcelLoader blockPlan, out IList<IValidationMessage> messages, ILocalization localization, Action next)
        {
            messages = new List<IValidationMessage>();
            foreach (var blockPlanSetting in blockPlan.Sheet<BlockPlanSetting>().Data)
            {
                if (blockPlanSetting.BlockSubjectCount == 0)
                {
                    messages.Add(String.Format(localization.GetLocalString("tsdv_BlockSizeZeroError"), blockPlanSetting.Blocks).ToValidationError());
                }
                else if(!TotalTierCountValid(blockPlanSetting))
                {
                    messages.Add(String.Format(localization.GetLocalString("tsdv_BlockValidationError"), blockPlanSetting.Blocks).ToValidationError());
                }
            }

            if (messages.Any())
            {
                return;
            }

            next();
        }

        private bool TotalTierCountValid(BlockPlanSetting block)
        {
            var totalTierSubjectCount = block.GetExtraProperties().Values.OfType<Int32>().Sum();

            return totalTierSubjectCount == block.BlockSubjectCount;
        }
    }
}
