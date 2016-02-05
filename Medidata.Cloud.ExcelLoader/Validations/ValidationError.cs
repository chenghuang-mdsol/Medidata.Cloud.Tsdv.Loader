using System.Diagnostics.CodeAnalysis;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    [ExcludeFromCodeCoverage]
    internal class ValidationError : IValidationError
    {
        public ValidationError(string message)
        {
            Message = message;
        }

        public IValidationRule ByWhom { get; set; }
        public string Message { get; private set; }
        public string Where { get; set; }
    }
}