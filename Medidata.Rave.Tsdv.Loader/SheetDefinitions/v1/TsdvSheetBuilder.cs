using System;
using System.ComponentModel;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    internal class TsdvSheetBuilder<T> : SheetBuilder<T> where T : class
    {
        private readonly ICellStyleProvider _styleProvider;
        private readonly IAutoFilterProvider _autoFilterProvider;
        private readonly string HeaderStyleName = "Output";
        private readonly string TextStyleName = "Normal";

        public TsdvSheetBuilder(ICellTypeValueConverterFactory converterFactory, ICellStyleProvider styleProvider, IAutoFilterProvider autoFilterProvider)
            : base(converterFactory)
        {
            if (styleProvider == null) throw new ArgumentNullException("styleProvider");
            _styleProvider = styleProvider;

            if (autoFilterProvider == null) throw new ArgumentNullException("autoFilterProvider");
            _autoFilterProvider = autoFilterProvider;
        }

        protected override Cell CreateCell(T model, PropertyDescriptor property)
        {
            var cell = base.CreateCell(model, property);
            cell.StyleIndex = _styleProvider.GetStyleIndex(Document, TextStyleName);
            return cell;
        }

        protected override Row CreateHeaderRow()
        {
            int columnIndex = 1;
            var row = base.CreateHeaderRow();
            foreach (var cell in row.Descendants<Cell>())
            {
                cell.StyleIndex = _styleProvider.GetStyleIndex(Document, HeaderStyleName);
                //Only header rows have cell references, because they are required only when you need to apply auto-filters.
                cell.CellReference = _autoFilterProvider.GetHeaderCellColumnString(columnIndex) + "1";
                columnIndex++;
            }
            return row;
        }

        protected override Worksheet CreateWorksheet()
        {
            Worksheet sheet = base.CreateWorksheet();
            var sheetData = sheet.Descendants<SheetData>().First();
            var autoFilter = _autoFilterProvider.CreateAutoFilter(sheetData);
            bool hasColumns = sheet.Descendants<Columns>().Any();
            if (hasColumns && autoFilter != null)
            {
                sheet.AppendChild(autoFilter);
            }
            return sheet;
        }
    }
}