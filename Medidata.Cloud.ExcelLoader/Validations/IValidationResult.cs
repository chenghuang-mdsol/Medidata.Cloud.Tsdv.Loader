using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationResult
    {
        IExcelParser ValidationTarget { get; }
        IList<IValidationMessage> Messages { get; }
    }
}