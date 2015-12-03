using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.CellStyleProviders;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.Helpers;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public class TsdvReportV1Loader : ITsdvReportLoader
    {
        private readonly ICellTypeValueConverterFactory _converterFactory;
        private readonly ILocalization _localization;
        private readonly ICellStyleProvider _styleProvider;
        public TsdvReportV1Loader(ICellTypeValueConverterFactory converterFactory, ILocalization localization, ICellStyleProvider styleProvider)
        {
            _converterFactory = converterFactory;
            _localization = localization;
            _styleProvider = styleProvider;
            BlockPlans = new List<IBlockPlan>();
            BlockPlanSettings = new List<IBlockPlanSetting>();
            CustomTiers = new List<ICustomTier>();
            TierFields = new List<ITierField>();
            TierForms = new List<ITierForm>();
            TierFolders = new List<ITierFolder>();
            ExcludedStatuses = new List<IExcludedStatus>();
            Rules = new List<IRule>();
        }

        public IList<IBlockPlan> BlockPlans { get; private set; }
        public IList<IBlockPlanSetting> BlockPlanSettings { get; private set; }
        public IList<ICustomTier> CustomTiers { get; private set; }
        public IList<ITierField> TierFields { get; private set; }
        public IList<ITierForm> TierForms { get; private set; }
        public IList<ITierFolder> TierFolders { get; private set; }
        public IList<IExcludedStatus> ExcludedStatuses { get; private set; }
        public IList<IRule> Rules { get; private set; }

        public void Save(Stream outStream)
        {
            var builder = new TsdvReportGenericBuilder(_converterFactory, _localization, _styleProvider);
            string headerStyleName = "Output";
            string textStyleName = "Normal";
            builder.AddSheet<IBlockPlan>("BlockPlans", headerStyleName, textStyleName).AddRange(BlockPlans);
            builder.AddSheet<IBlockPlanSetting>("BlockPlanSettings", headerStyleName, textStyleName).AddRange(BlockPlanSettings);
            builder.AddSheet<ICustomTier>("CustomTiers", headerStyleName, textStyleName).AddRange(CustomTiers);
            builder.AddSheet<ITierField>("TierFields", headerStyleName, textStyleName).AddRange(TierFields);
            builder.AddSheet<ITierForm>("TierForms", headerStyleName, textStyleName).AddRange(TierForms);
            builder.AddSheet<ITierFolder>("TierFolders", headerStyleName, textStyleName).AddRange(TierFolders);
            builder.AddSheet<IExcludedStatus>("ExcludedStatuses", headerStyleName, textStyleName).AddRange(ExcludedStatuses);
            builder.AddSheet<IRule>("Rules", headerStyleName, textStyleName).AddRange(Rules);

            builder.Save(outStream);
        }

        public void Load(Stream source)
        {
            var parser = new ExcelParser(_converterFactory);
            parser.Load(source);

            BlockPlans = parser.GetObjects<IBlockPlan>("BlockPlans").ToList();
            BlockPlanSettings = parser.GetObjects<IBlockPlanSetting>("BlockPlanSettings").ToList();
            CustomTiers = parser.GetObjects<ICustomTier>("CustomTiers").ToList();
            TierFields = parser.GetObjects<ITierField>("TierFields").ToList();
            TierForms = parser.GetObjects<ITierForm>("TierForms").ToList();
            TierFolders = parser.GetObjects<ITierFolder>("TierFolders").ToList();
            ExcludedStatuses = parser.GetObjects<IExcludedStatus>("ExcludedStatuses").ToList();
            Rules = parser.GetObjects<IRule>("Rules").ToList();
        }
    }
}