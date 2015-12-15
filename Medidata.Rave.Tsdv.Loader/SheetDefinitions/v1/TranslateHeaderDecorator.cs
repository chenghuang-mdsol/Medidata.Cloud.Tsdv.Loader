using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public class TranslateHeaderDecorator : ISheetBuilderDecorator
    {
        private readonly ILocalization _localization;

        public TranslateHeaderDecorator(ILocalization localization)
        {
            _localization = localization;
        }

        public ISheetBuilder Decorate(ISheetBuilder target)
        {
            var originalFunc = target.BuildSheet;
            target.BuildSheet = (objects, sheetDefinition, doc) =>
            {
                originalFunc(objects, sheetDefinition, doc);
                var sheetData = doc.GetSheetDataByName(sheetDefinition.Name);
                var headerRow = sheetData.Descendants<Row>().FirstOrDefault();
                if (headerRow == null) return;
                foreach (var cell in headerRow.Descendants<Cell>())
                {
                    var value = cell.CellValue.InnerText;
                    cell.CellValue = new CellValue(_localization.GetLocalString(value));
                }
            };

            return target;
        }
    }
}