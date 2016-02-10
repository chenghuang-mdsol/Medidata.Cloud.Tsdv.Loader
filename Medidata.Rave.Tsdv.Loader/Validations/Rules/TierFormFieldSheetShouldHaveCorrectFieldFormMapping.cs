using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.Validations.Rules
{
    public class TierFormFieldSheetShouldHaveCorrectFieldFormMapping : I18NValidationRuleBase
    {
        private readonly IValidationHelper _helper;

        public TierFormFieldSheetShouldHaveCorrectFieldFormMapping(IValidationHelper helper, ILocalization localization)
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
                            let fieldOid = tierField.FieldOid
                            where !_helper.ExistsFormField(formOid, fieldOid, context)
                            select CreateErrorMessage("Cannot find field OID '{0}' in form '{1}'.", fieldOid, formOid))
                .ToArray();
            shouldContinue = messages.Length == 0;
            return messages;
        }
    }
}