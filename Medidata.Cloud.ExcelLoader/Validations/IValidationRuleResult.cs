using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationRuleResult
    {
        IList<IValidationMessage> Messages { get; }
        bool ShouldContinue { get; }
    }
}