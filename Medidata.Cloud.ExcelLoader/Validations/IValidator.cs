using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidator
    {
        IValidationResult Validate(IExcelLoader excelLoader, IDictionary<string, object> context);
    }
}