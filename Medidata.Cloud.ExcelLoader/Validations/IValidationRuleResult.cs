using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationRuleResult
    {
        IEnumerable<IValidationMessage> Messages { get; }
        bool ShouldContinue { get; }
    }
}