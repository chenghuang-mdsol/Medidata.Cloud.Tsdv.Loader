namespace Medidata.Cloud.ExcelLoader.Validations
{
    public interface IValidationRule
    {
        IValidationRuleResult Check(IExcelParser blockPlan);
    }
}