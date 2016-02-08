using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    [ExcludeFromCodeCoverage]
    internal class ValidationResult : IValidationResult
    {
        public IExcelLoader ValidationTarget { get; set; }
        public IEnumerable<IValidationMessage> Messages { get; set; }
    }
}