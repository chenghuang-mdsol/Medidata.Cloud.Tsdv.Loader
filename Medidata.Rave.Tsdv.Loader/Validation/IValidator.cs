using Medidata.Interfaces.TSDV;

namespace Medidata.Rave.Tsdv.Loader.Validation
{
    public interface IValidator
    {
        IValidationResult Validate(IBlockPlan blockPlan);
    }
}