namespace Medidata.Rave.Tsdv.Loader.Validation.Rules
{
    internal class ValidationRuleResult : IValidationRuleResult
    {
        public IValidationMessage Message { get; set; }
        public bool ShouldContinue { get; set; }
    }
}