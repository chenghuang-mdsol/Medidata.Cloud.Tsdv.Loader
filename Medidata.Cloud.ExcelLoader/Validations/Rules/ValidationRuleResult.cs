using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Medidata.Cloud.ExcelLoader.Validations.Rules
{
    [ExcludeFromCodeCoverage]
    internal class ValidationRuleResult : IValidationRuleResult
    {
        public IList<IValidationMessage> Messages { get; set; }
        public bool ShouldContinue { get; set; }
    }
}