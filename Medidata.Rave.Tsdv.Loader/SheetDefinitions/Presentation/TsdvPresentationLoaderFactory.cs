using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.CellTypeConverters;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;
using Medidata.Cloud.ExcelLoader.DefinedNamedRange;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    public class TsdvPresentationLoaderFactory : ITsdvExcelLoaderFactory
    {
        private readonly ILocalization _localization;
        private readonly INamedRangeProvider _namedRangeProvider;
        public TsdvPresentationLoaderFactory(ILocalization localization, INamedRangeProvider namedRangeProvider = null)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
            _namedRangeProvider = namedRangeProvider;
        }

        public IExcelLoader Create(TsdvLoaderSupportedVersion version)
        {
            var loader = CreateTsdvExcelLoader();
            loader = DefineTsdvSheets(version, loader);
            return loader;
        }

        [ExcludeFromCodeCoverage]
        protected virtual IEnumerable<ICellTypeValueConverter> GetCustomCellTypeValueConverters()
        {
            return Enumerable.Empty<ICellTypeValueConverter>();
        }

        [ExcludeFromCodeCoverage]
        internal virtual IExcelLoader CreateTsdvExcelLoader()
        {
            var customConverters = GetCustomCellTypeValueConverters().ToArray();
            var converterManager = new CellTypeValueConverterManager(customConverters);
            //var excelBuilder = new AutoCopyrightCoveredExcelBuilder();
            var excelBuilder = new AutoCopyrightCoveredResourcedExcelBuilder(_namedRangeProvider);
            var excelParser = new ExcelParser();

            var sheetDecorators = new ISheetBuilderDecorator[]
                                  {
                                      new HeaderSheetDecorator(),
                                      new TranslateHeaderDecorator(_localization),
                                      new TextStyleSheetDecorator("Normal"),
                                      new HeaderStyleSheetDecorator("Output"),
                                      new AutoFilterSheetDecorator(),
                                      new AutoFitColumnSheetDecorator(),
                                      new MdsolVersionSheetDecorator(), 
                                      new ColumnDataValidationSheetDecorator(), 
                                  };
            var sheetBuilder = new SheetBuilder(converterManager, sheetDecorators);
            var sheetParser = new SheetParser(converterManager);

            return new ExcelLoader(excelBuilder, excelParser, sheetBuilder, sheetParser);
        }

        protected virtual IExcelLoader DefineTsdvSheets(TsdvLoaderSupportedVersion version, IExcelLoader loader)
        {
            if(version != TsdvLoaderSupportedVersion.Presentation)
                throw new NotSupportedException(string.Format("'{0}' isn't a supported name", version));

            loader.Sheet<BlockPlan>().Definition.HeaderRowCount = 1;
            loader.Sheet<BlockPlanSetting>().Definition.HeaderRowCount = 1;
            loader.Sheet<CustomTier>().Definition.HeaderRowCount = 1;
            loader.Sheet<TierField>().Definition.HeaderRowCount = 1;
            loader.Sheet<TierForm>().Definition.HeaderRowCount = 1;
            loader.Sheet<TierFolder>().Definition.HeaderRowCount = 1;
            loader.Sheet<ExcludedStatus>().Definition.HeaderRowCount = 1;
            loader.Sheet<Rule>().Definition.HeaderRowCount = 1;
            return loader;
        }
    }
}