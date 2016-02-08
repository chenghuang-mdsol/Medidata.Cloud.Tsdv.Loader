namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationResult
    {
        IExcelLoader ValidationTarget { get; }
        IValidationMessageCollection Messages { get; }
    }
}