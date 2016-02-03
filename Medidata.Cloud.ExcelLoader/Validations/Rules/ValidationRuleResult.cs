using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations.Rules
{
    internal class ValidationRuleResult : IValidationRuleResult
    {
        public IList<IValidationMessage> Messages { get; set; }
        public bool ShouldContinue { get; set; }
    }
}