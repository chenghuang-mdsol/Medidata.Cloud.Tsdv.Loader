using System.Diagnostics.CodeAnalysis;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    [ExcludeFromCodeCoverage]
    internal class ValidationWarning : IValidationWarning
    {
        public ValidationWarning(string message)
        {
            Message = message;
        }

        public IValidationRule ByWhom { get; set; }
        public string Message { get; private set; }
        public string Where { get; set; }
    }
}