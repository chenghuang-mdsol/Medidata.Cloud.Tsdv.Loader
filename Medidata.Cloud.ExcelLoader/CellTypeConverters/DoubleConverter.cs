namespace Medidata.Cloud.ExcelLoader.CellTypeConverters
{
    internal class DoubleConverter : NumberConverter<double>
    {
        protected override double GetCSharpValueImpl(string cellValue)
        {
            double value;
            double.TryParse(cellValue, out value);
            return value;
        }
    }
}