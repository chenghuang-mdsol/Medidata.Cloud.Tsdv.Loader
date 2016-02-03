namespace Medidata.Cloud.ExcelLoader
{
    public class ColumnDefinition : IColumnDefinition
    {
        public string PropertyName { get; set; }
        public string Header { get; set; }
        public bool ExtraProperty { get; set; }
    }
}