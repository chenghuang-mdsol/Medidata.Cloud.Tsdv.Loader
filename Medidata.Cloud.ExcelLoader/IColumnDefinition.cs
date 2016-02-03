namespace Medidata.Cloud.ExcelLoader
{
    public interface IColumnDefinition
    {
        string PropertyName { get; set; }
        string Header { get; set; }
        bool ExtraProperty { get; set; }
    }
}