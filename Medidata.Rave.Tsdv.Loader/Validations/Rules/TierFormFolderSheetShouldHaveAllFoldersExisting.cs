using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class TierFormFolderSheetShouldHaveAllFoldersExisting : I18NValidationRuleBase
    {
        private readonly IValidationHelper _helper;

        public TierFormFolderSheetShouldHaveAllFoldersExisting(IValidationHelper helper, ILocalization localization)
            : base(localization)
        {
            if (helper == null) throw new ArgumentNullException("helper");
            _helper = helper;
        }

        protected override IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader,
                                                                    IDictionary<string, object> context,
                                                                    out bool shouldContinue)
        {
            var messages = (from tierFolder in excelLoader.Sheet<TierFormFolder>().Definition.ColumnDefinitions
                            where tierFolder.ExtraProperty
                            let folderOid = tierFolder.PropertyName
                            where !_helper.ExistsFolderOid(folderOid, context)
                            select CreateErrorMessage("Cannot find folder OID '{0}'.", folderOid))
                .ToArray();

            shouldContinue = messages.Length == 0;
            return messages;
        }
    }
}