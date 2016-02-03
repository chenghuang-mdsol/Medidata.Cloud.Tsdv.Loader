using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader.SheetDecorators
{
    public class AutoFitColumnSheetDecorator : ISheetBuilderDecorator
    {
        public ISheetBuilder Decorate(ISheetBuilder target)
        {
            var originalFunc = target.BuildSheet;
            target.BuildSheet = (models, sheetDefinition, doc) =>
            {
                originalFunc(models, sheetDefinition, doc);

                AddColumns(doc, sheetDefinition.Name);
            };
            return target;
        }

        private void AddColumns(SpreadsheetDocument doc, string sheetName)
        {
            var sheetData = doc.GetSheetDataByName(sheetName);
            var headerRow = sheetData.Descendants<Row>().FirstOrDefault();
            if (headerRow == null) return;

            var colCount = headerRow.Descendants<Cell>().Count();
            var columnRange = Enumerable.Range(0, colCount).Select(i => CreateColumn(doc, sheetData, i));
            var columns = new Columns(columnRange);

            var sheet = doc.GetWorksheetByName(sheetName);
            sheet.InsertAt(columns, 0);
        }

        private Column CreateColumn(SpreadsheetDocument doc, SheetData sheetData, int colIndex)
        {
            var column = new Column
                         {
                             Min = (uint) (colIndex + 1),
                             Max = (uint) (colIndex + 1),
                             Width = CalculateColumnWidth(doc, sheetData, colIndex),
                             CustomWidth = true
                         };
            return column;
        }

        private double CalculateColumnWidth(SpreadsheetDocument doc, SheetData sheetData, int colIndex)
        {
            // See http://refactorsaurusrex.com/how-to-get-the-custom-format-string-for-an-excelopenxml-cell/
            var allCellWidth = from cell in GetColumnCells(sheetData, colIndex)
                               let cellFormat = GetCellFormat(doc, cell)
                               let font = GetFont(doc, cellFormat)
                               let fontName = font.FontName.Val
                               let fontSize = int.Parse(font.FontSize.Val)
                               let width = CalculateTextWidth(fontName, fontSize, cell.InnerText)
                               select width;
            return allCellWidth.Max();
        }

        private static double CalculateTextWidth(string font, int fontSize, string text)
        {
            // The algorithm is from below link
            // https://social.msdn.microsoft.com/Forums/office/en-US/28aae308-55cb-479f-9b58-d1797ed46a73/solution-how-to-autofit-excel-content?forum=oxmlsdk
            var stringFont = new System.Drawing.Font(font, fontSize);
            var textSize = TextRenderer.MeasureText(text, stringFont);
            var width = (textSize.Width / (double) 7 * 256 - 18) / 256;
            width = (double) decimal.Round((decimal) width + 0.2M + 2.29M, 2);
            return width;
        }

        private IEnumerable<Cell> GetColumnCells(SheetData sheetData, int colIndex)
        {
            var cells = sheetData.Descendants<Row>()
                                 .Select(x => x.Descendants<Cell>().ElementAt(colIndex));
            return cells;
        }

        private CellFormat GetCellFormat(SpreadsheetDocument doc, Cell cell)
        {
            var index = (int) cell.StyleIndex.Value;
            var cellFormat = doc.WorkbookPart
                                .WorkbookStylesPart
                                .Stylesheet
                                .CellFormats
                                .Elements<CellFormat>()
                                .ElementAt(index);
            return cellFormat;
        }

        private Font GetFont(SpreadsheetDocument doc, CellFormat cellFormat)
        {
            var index = (int) cellFormat.FontId.Value;
            var font = doc.WorkbookPart
                          .WorkbookStylesPart
                          .Stylesheet
                          .Fonts
                          .Elements<Font>()
                          .ElementAt(index);
            return font;
        }
    }
}