namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidator
    {
        IValidationResult Validate(IExcelLoader excelLoader);
    }
}