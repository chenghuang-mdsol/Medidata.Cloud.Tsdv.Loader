using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Medidata.Cloud.ExcelLoader.CellTypeConverters
{
    public abstract class CellTypeValueBaseConverter<T> : ICellTypeValueConverter
    {
        protected CellTypeValueBaseConverter(CellValues cellType)
        {
            CellType = cellType;
        }

        public CellValues CellType { get; private set; }

        public bool TryConvertToCellValue(object csharpValue, out string cellValue)
        {
            if (csharpValue == null)
            {
                cellValue = null;
                return true;
            }

            T value;
            try
            {
                value = (T) csharpValue;
            }
            catch
            {
                cellValue = null;
                return false;
            }

            return TryGetCellValueImpl(value, out cellValue);
        }

        public bool TryConvertToCSharpValue(string cellValue, out object csharpValue)
        {
            T value;
            var result = TryGetCSharpValueImpl(cellValue, out value);
            if (result)
            {
                csharpValue = value;
                return true;
            }

            csharpValue = null;
            return false;
        }

        protected bool IsNumeric(string value)
        {
            var isNumericRegex = new Regex("^(" +
                                           /*Hex*/ @"0x[0-9a-f]+" + "|" +
                                           /*Bin*/ @"0b[01]+" + "|" +
                                           /*Oct*/ @"0[0-7]*" + "|" +
                                           /*Dec*/ @"((?!0)|[-+]|(?=0+\.))(\d*\.)?\d+(e\d+)?" + ")$");
            return isNumericRegex.IsMatch(value);
        }

        protected abstract bool TryGetCellValueImpl(T csharpValue, out string cellValue);

        protected abstract bool TryGetCSharpValueImpl(string cellValue, out T csharpValue);
    }
}