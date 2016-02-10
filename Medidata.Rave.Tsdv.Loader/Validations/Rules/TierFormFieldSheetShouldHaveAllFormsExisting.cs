using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class TierFormFieldSheetShouldHaveAllFormsExisting : I18NValidationRuleBase
    {
        private readonly IValidationHelper _helper;

        public TierFormFieldSheetShouldHaveAllFormsExisting(IValidationHelper helper, ILocalization localization)
            : base(localization)
        {
            if (helper == null) throw new ArgumentNullException("helper");
            _helper = helper;
        }

        protected override IEnumerable<IValidationMessage> Validate(IExcelLoader excelLoader,
                                                                    IDictionary<string, object> context,
                                                                    out bool shouldContinue)
        {
            var messages = (from tierField in excelLoader.Sheet<TierFormField>().Data
                            let formOid = tierField.FormOid
                            where !_helper.ExistsFormOid(formOid, context)
                            select CreateErrorMessage("Cannot find form OID '{0}'.", formOid))
                .ToArray();

            shouldContinue = messages.Length == 0;
            return messages;
        }
    }
}