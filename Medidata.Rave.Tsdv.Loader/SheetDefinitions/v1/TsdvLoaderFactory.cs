using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public class TsdvLoaderFactory : TsdvPresentationLoaderFactory
    {
        public TsdvLoaderFactory(ILocalization localization) : base(localization) {}

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