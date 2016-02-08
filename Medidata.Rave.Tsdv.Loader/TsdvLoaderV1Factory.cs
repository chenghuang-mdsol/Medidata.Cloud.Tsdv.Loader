using System.Collections.Generic;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.ColumnResources;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvLoaderV1Factory : TsdvPresentationLoaderFactory
    {
        public TsdvLoaderV1Factory(ILocalization localization, IColumnResourceManager resources) : base(localization,resources) {}

        protected override IExcelLoader DefineTsdvSheets(TsdvLoaderSupportedVersion version, IExcelLoader loader)
        {
            if (version != TsdvLoaderSupportedVersion.V1) return base.DefineTsdvSheets(version, loader);

            loader.Sheet<BlockPlanSetting>().Definition.HeaderRowCount = 1;
            loader.Sheet<CustomTier>().Definition.HeaderRowCount = 1;
            loader.Sheet<TierForm>().Definition.HeaderRowCount = 1;
            loader.Sheet<TierField>().Definition.HeaderRowCount = 1;
            loader.Sheet<TierFolder>().Definition.HeaderRowCount = 1;
            loader.Sheet<Rule>().Definition.HeaderRowCount = 1;
            return loader;
        }
    }
}