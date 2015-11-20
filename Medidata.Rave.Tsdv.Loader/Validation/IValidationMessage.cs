namespace Medidata.Rave.Tsdv.Loader.Validation
{
    public interface IValidationMessage
    {
        IValidationRule ByWhom { get; }
        string Message { get; }
        string Where { get; }
    }
}