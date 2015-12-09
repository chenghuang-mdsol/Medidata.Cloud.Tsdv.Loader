using System;
using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    internal class SheetBuilderFactory : ISheetBuilderFactory
    {
        private readonly ICellTypeValueConverterFactory _converterFactory;
        private readonly ICellStyleProvider _styleProvider;
        private readonly IAutoFilterProvider _autoFilterProvider;
        public SheetBuilderFactory(ICellTypeValueConverterFactory converterFactory, ICellStyleProvider styleProvider, IAutoFilterProvider autoFilterProvider)
        {
            if (converterFactory == null) throw new ArgumentNullException("converterFactory");
            if (styleProvider == null) throw new ArgumentNullException("styleProvider");
            _converterFactory = converterFactory;
            _styleProvider = styleProvider;
            _autoFilterProvider = autoFilterProvider;
        }

        public ISheetBuilder Create<T>() where T : class
        {
            return new TsdvSheetBuilder<T>(_converterFactory, _styleProvider,_autoFilterProvider);
        }
    }
}