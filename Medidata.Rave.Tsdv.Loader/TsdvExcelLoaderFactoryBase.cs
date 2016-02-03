using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader
{
    public abstract class TsdvExcelLoaderFactoryBase : ITsdvExcelLoaderFactory
    {
        private readonly ILocalization _localization;

        protected TsdvExcelLoaderFactoryBase(ILocalization localization)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        public virtual IExcelLoader Create()
        {
            var loader = CreateTsdvExcelLoader();
            loader = DefineTsdvSheets(loader);
            return loader;
        }

        private IExcelLoader CreateTsdvExcelLoader()
        {
            var converterManager = new CellTypeValueConverterManager();
            var excelBuilder = new AutoCopyrightCoveredExcelBuilder();
            var excelParser = new ExcelParser();

            var sheetDecorators = new ISheetBuilderDecorator[]
                                  {
                                      new HeaderSheetDecorator(),
                                      new TranslateHeaderDecorator(_localization),
                                      new TextStyleSheetDecorator("Normal"),
                                      new HeaderStyleSheetDecorator("Output"),
                                      new AutoFilterSheetDecorator(),
                                      new AutoFitColumnSheetDecorator(),
                                      new MdsolVersionSheetDecorator()
                                  };
            var sheetBuilder = new SheetBuilder(converterManager, sheetDecorators);
            var sheetParser = new SheetParser(converterManager);

            return new ExcelLoader(excelBuilder, excelParser, sheetBuilder, sheetParser);
        }

        protected abstract IExcelLoader DefineTsdvSheets(IExcelLoader loader);
    }
}