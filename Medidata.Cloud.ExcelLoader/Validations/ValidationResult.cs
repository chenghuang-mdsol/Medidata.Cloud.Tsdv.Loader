using System;
using System.Diagnostics.CodeAnalysis;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    [ExcludeFromCodeCoverage]
    internal class ValidationResult : IValidationResult
    {
        public IExcelLoader ValidationTarget { get; set; }
        public IValidationMessageCollection Messages { get; set; }
    }
}