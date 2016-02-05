using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    [ExcludeFromCodeCoverage]
    internal class ValidationResult : IValidationResult
    {
        public ValidationResult()
        {
            Messages = new List<IValidationMessage>();
        }

        public IExcelLoader ValidationTarget { get; set; }
        public IList<IValidationMessage> Messages { get; private set; }
    }
}