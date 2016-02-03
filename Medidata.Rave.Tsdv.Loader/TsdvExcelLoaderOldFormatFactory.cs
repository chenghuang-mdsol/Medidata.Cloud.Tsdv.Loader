using System;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.OldFormat;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoaderOldFormatFactory : ITsdvExcelLoaderFactory
    {
        private readonly ILocalization _localization;

        public TsdvExcelLoaderOldFormatFactory(ILocalization localization)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        public IExcelLoader Create(TsdvLoaderSupportedVersion version)
        {
            var loader = CreateTsdvExcelLoader();
            loader = DefineTsdvSheets(version, loader);
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

        protected virtual IExcelLoader DefineTsdvSheets(TsdvLoaderSupportedVersion version, IExcelLoader loader)
        {
            if(version != TsdvLoaderSupportedVersion.OldFormat)
                throw new NotSupportedException(string.Format("'{0}' isn't a supported name", version));

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