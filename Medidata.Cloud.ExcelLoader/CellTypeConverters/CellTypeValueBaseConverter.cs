using System;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Medidata.Cloud.ExcelLoader.CellTypeConverters
{
    internal abstract class CellTypeValueBaseConverter<T> : ICellTypeValueConverter
    {
        protected CellTypeValueBaseConverter(CellValues cellType)
        {
            CellType = cellType;
            CSharpType = typeof (T);
        }

        public CellValues CellType { get; private set; }
        public Type CSharpType { get; private set; }

        public string GetCellValue(object csharpValue)
        {
            return csharpValue == null ? null : GetCellValueImpl((T) csharpValue);
        }

        public object GetCSharpValue(string cellValue)
        {
            return GetCSharpValueImpl(cellValue);
        }

        protected abstract string GetCellValueImpl(T csharpValue);

        protected abstract T GetCSharpValueImpl(string cellValue);
    }
}