using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoaderV1Factory : TsdvExcelLoaderOldFormatFactory
    {
        public TsdvExcelLoaderV1Factory(ILocalization localization) : base(localization) {}

        protected override IExcelLoader DefineTsdvSheets(TsdvLoaderSupportedVersion version, IExcelLoader loader)
        {
            if (version != TsdvLoaderSupportedVersion.V1) return base.DefineTsdvSheets(version, loader);

            loader.Sheet<BlockPlanSetting>();
            loader.Sheet<CustomTier>();
            loader.Sheet<TierForm>();
            loader.Sheet<TierField>();
            loader.Sheet<TierFolder>();
            loader.Sheet<Rule>();
            return loader;
        }
    }
}