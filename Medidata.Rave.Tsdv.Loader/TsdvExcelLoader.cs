using Medidata.Cloud.ExcelLoader;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoader : ExcelLoader
    {
        public TsdvExcelLoader(IExcelBuilder builder, IExcelParser parser, ISheetBuilder sheetBuilder,
                               ISheetParser sheetParser) : base(builder, parser, sheetBuilder, sheetParser)
        {
            SheetDefinition<BlockPlan>();
            SheetDefinition<BlockPlanSetting>();
            SheetDefinition<CustomTier>();
            SheetDefinition<TierField>();
            SheetDefinition<TierForm>();
            SheetDefinition<TierFolder>();
            SheetDefinition<ExcludedStatus>();
            SheetDefinition<Rule>();
        }
    }
}