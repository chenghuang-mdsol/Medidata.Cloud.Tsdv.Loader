using System.Collections.Generic;
using Medidata.Interfaces.TSDV;

namespace Medidata.Rave.Tsdv.Loader.Validation
{
    internal class ValidationResult : IValidationResult
    {
        public ValidationResult()
        {
            Messages = new List<IValidationMessage>();
        }

        public IBlockPlan ValidationTarget { get; set; }
        public IList<IValidationMessage> Messages { get; private set; }
    }
}