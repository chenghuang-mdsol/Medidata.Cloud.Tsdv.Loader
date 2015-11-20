using System.Collections.Generic;
using Medidata.Interfaces.TSDV;

namespace Medidata.Rave.Tsdv.Loader.Validation
{
    public interface IValidationResult
    {
        IBlockPlan ValidationTarget { get; }
        IList<IValidationMessage> Messages { get; }
    }
}