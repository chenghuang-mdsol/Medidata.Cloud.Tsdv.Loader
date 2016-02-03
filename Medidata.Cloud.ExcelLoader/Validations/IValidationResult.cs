using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationResult
    {
        IExcelLoader ValidationTarget { get; }
        IList<IValidationMessage> Messages { get; }
    }
}