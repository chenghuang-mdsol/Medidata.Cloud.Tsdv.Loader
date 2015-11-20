namespace Medidata.Rave.Tsdv.Loader.Validation
{
    public interface IValidationRuleResult
    {
        IValidationMessage Message { get; }
        bool ShouldContinue { get; }
    }
}