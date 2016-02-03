using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
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