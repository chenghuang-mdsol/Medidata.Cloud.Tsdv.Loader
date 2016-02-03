using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.OldFormat;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoaderOldFormatFactory : TsdvExcelLoaderFactoryBase
    {
        public TsdvExcelLoaderOldFormatFactory(ILocalization localization) : base(localization) {}

        protected override IExcelLoader DefineTsdvSheets(IExcelLoader loader)
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