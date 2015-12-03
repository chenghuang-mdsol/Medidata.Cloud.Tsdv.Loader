using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.CellStyleProviders;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.Helpers;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public class TsdvReportLoader : ITsdvReportLoader
    {
        private IExcelBuilder _builder;
        private IExcelParser _parser;

        public TsdvReportLoader(ILocalization localization)
        {
            var cellTypeValueConverterFactory = new CellTypeValueConverterFactory();
            var styleProvider = new EmbeddedCellStyleProvider();
            var sheetBuilderFactory = new SheetBuilderFactory(cellTypeValueConverterFactory, styleProvider);
            var builder = new TsdvReportGenericBuilder(styleProvider, sheetBuilderFactory, localization);
            var parser = new ExcelParser(cellTypeValueConverterFactory);

            Initialize(builder, parser);
        }

        internal TsdvReportLoader(IExcelBuilder builder, IExcelParser parser)
        {
            Initialize(builder, parser);
        }

        private void Initialize(IExcelBuilder builder, IExcelParser parser)
        {
            _builder = builder;
            _parser = parser;
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
            _builder.AddSheet<IBlockPlan>("BlockPlans").AddRange(BlockPlans);
            _builder.AddSheet<IBlockPlanSetting>("BlockPlanSettings").AddRange(BlockPlanSettings);
            _builder.AddSheet<ICustomTier>("CustomTiers").AddRange(CustomTiers);
            _builder.AddSheet<ITierField>("TierFields").AddRange(TierFields);
            _builder.AddSheet<ITierForm>("TierForms").AddRange(TierForms);
            _builder.AddSheet<ITierFolder>("TierFolders").AddRange(TierFolders);
            _builder.AddSheet<IExcludedStatus>("ExcludedStatuses").AddRange(ExcludedStatuses);
            _builder.AddSheet<IRule>("Rules").AddRange(Rules);

            _builder.Save(outStream);
        }

        public void Load(Stream source)
        {
            _parser.Load(source);

            BlockPlans = _parser.GetObjects<IBlockPlan>("BlockPlans").ToList();
            BlockPlanSettings = _parser.GetObjects<IBlockPlanSetting>("BlockPlanSettings").ToList();
            CustomTiers = _parser.GetObjects<ICustomTier>("CustomTiers").ToList();
            TierFields = _parser.GetObjects<ITierField>("TierFields").ToList();
            TierForms = _parser.GetObjects<ITierForm>("TierForms").ToList();
            TierFolders = _parser.GetObjects<ITierFolder>("TierFolders").ToList();
            ExcludedStatuses = _parser.GetObjects<IExcludedStatus>("ExcludedStatuses").ToList();
            Rules = _parser.GetObjects<IRule>("Rules").ToList();
        }
    }
}