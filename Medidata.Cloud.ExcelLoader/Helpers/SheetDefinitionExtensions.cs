namespace Medidata.Cloud.ExcelLoader.Helpers
{
    public static class SheetDefinitionExtensions
    {
        public static ISheetDefinition AddColumn(this ISheetDefinition target, string propertyName)
        {
            return AddColumn(target, propertyName, null);
        }

        public static ISheetDefinition AddColumn(this ISheetDefinition target, string propertyName, string header)
        {
            return target.AddColumn(new ColumnDefinition {PropertyName = propertyName, Header = header, ExtraProperty = true});
        }
    }
}