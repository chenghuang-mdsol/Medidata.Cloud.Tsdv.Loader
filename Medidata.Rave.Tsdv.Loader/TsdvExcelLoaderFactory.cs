using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoaderFactory: ITsdvExcelLoaderFactory
    {
        private readonly ILocalization _localization;

        public TsdvExcelLoaderFactory(ILocalization localization)
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

        private IExcelLoader DefineTsdvSheets(IExcelLoader loader)
        {
            loader.Sheet<BlockPlan>();
            loader.Sheet<BlockPlanSetting>();
            loader.Sheet<CustomTier>();
            loader.Sheet<TierField>();
            loader.Sheet<TierForm>();
            loader.Sheet<TierFolder>();
            loader.Sheet<ExcludedStatus>();
            loader.Sheet<Rule>();
            return loader;
        }
    }
}