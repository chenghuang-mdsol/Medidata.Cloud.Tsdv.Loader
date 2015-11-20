using Medidata.Interfaces.TSDV;

namespace Medidata.Rave.Tsdv.Loader.Validation
{
    public interface IValidationRule
    {
        IValidationRuleResult Check(IBlockPlan blockPlan);
    }
}