using Medidata.Rave.Tsdv.Loader.Validation;

namespace Medidata.Rave.Tsdv.Loader.Helpers
{
    public static class StringValidationMessageExtensions
    {
        public static IValidationMessage ToValidationWarning(this string target)
        {
            return new ValidationWarning(target);
        }

        public static IValidationMessage ToValidationError(this string target)
        {
            return new ValidationError(target);
        }
    }
}