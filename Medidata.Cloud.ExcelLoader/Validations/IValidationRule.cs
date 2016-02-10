using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationRule
    {
        IValidationRuleResult Check(IExcelLoader excelLoader, IDictionary<string, object> context);
    }
}