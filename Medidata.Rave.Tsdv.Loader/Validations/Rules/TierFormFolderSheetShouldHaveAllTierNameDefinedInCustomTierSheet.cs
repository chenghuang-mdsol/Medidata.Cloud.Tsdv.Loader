using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class TierFormFolderSheetShouldHaveAllTierNameDefinedInCustomTierSheet : I18NValidationRuleBase
    {
        public TierFormFolderSheetShouldHaveAllTierNameDefinedInCustomTierSheet(ILocalization localization)
            : base(localization) {}

        protected override IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader,
                                                                    IDictionary<string, object> context,
                                                                    out bool shouldContinue)
        {
            var namesInCustomTier = excelLoader.Sheet<CustomTier>().Data.Select(x => x.TierName);
            var namesInTierFormFolder = excelLoader.Sheet<TierFormFolder>().Data.Select(x => x.TierName);
            var orphanNames = namesInTierFormFolder.Except(namesInCustomTier).ToArray();
            shouldContinue = orphanNames.Length == 0;
            return
                orphanNames.Select(x => CreateErrorMessage("'{0}' tier in TierForms is not defined in CustomTier.", x));
        }
    }
}