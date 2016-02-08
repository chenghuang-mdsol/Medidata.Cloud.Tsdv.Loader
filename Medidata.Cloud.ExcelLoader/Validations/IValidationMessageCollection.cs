using System.Collections.Generic;

namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationMessageCollection: IEnumerable<IValidationMessage>
    {
        IValidationMessageCollection Add(IValidationMessage item);
        IValidationMessageCollection AddRange(IEnumerable<IValidationMessage> items);
    }
}