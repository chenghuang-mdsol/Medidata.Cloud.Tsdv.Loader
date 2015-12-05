using System;
using System.ComponentModel;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    internal class TsdvSheetBuilder<T> : SheetBuilder<T> where T : class
    {
        private readonly ICellStyleProvider _styleProvider;
        private readonly string HeaderStyleName = "Output";
        private readonly string TextStyleName = "Normal";

        public TsdvSheetBuilder(ICellTypeValueConverterFactory converterFactory, ICellStyleProvider styleProvider)
            : base(converterFactory)
        {
            if (styleProvider == null) throw new ArgumentNullException("styleProvider");
            _styleProvider = styleProvider;
        }

        protected override Cell CreateCell(T model, PropertyDescriptor property)
        {
            var cell = base.CreateCell(model, property);
            cell.StyleIndex = _styleProvider.GetStyleIndex(Document, TextStyleName);
            return cell;
        }

        protected override Row CreateHeaderRow()
        {
            var row = base.CreateHeaderRow();
            foreach (var cell in row.Descendants<Cell>())
            {
                cell.StyleIndex = _styleProvider.GetStyleIndex(Document, HeaderStyleName);
            }
            return row;
        }
    }
}