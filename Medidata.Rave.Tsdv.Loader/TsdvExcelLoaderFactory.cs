﻿using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TsdvExcelLoaderFactory : TsdvExcelLoaderFactoryBase
    {
        public TsdvExcelLoaderFactory(ILocalization localization) : base(localization) {}
        protected override IExcelLoader DefineTsdvSheets(IExcelLoader loader)
        {
            loader.Sheet<BlockPlanSetting>();
            loader.Sheet<CustomTier>();
            loader.Sheet<TierForm>();
            loader.Sheet<TierField>();
            loader.Sheet<TierFolder>();
            loader.Sheet<Rule>();
            loader.Sheet<RuleStep>();
            loader.Sheet<RuleAction>();
            return loader;
        }
    }
}