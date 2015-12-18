using Medidata.Cloud.ExcelLoader;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoader : ExcelLoader
    {
        public TsdvExcelLoader(IExcelBuilder builder, IExcelParser parser, ISheetBuilder sheetBuilder,
                               ISheetParser sheetParser) : base(builder, parser, sheetBuilder, sheetParser)
        {
            Sheet<BlockPlan>();
            Sheet<BlockPlanSetting>();
            Sheet<CustomTier>();
            Sheet<TierField>();
            Sheet<TierForm>();
            Sheet<TierFolder>();
            Sheet<ExcludedStatus>();
            Sheet<Rule>();
        }
    }
}