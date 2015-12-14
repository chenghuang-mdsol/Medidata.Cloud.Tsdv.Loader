using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public class TsdvReportLoader : ITsdvReportLoader
    {
        private IExcelBuilder _builder;
        private IExcelParser _parser;
        private readonly ISheetBuilder _defaultSheetBuilder;

        public TsdvReportLoader(ILocalization localization)
        {
            var cellTypeValueConverterFactory = new CellTypeValueConverterFactory();

            _defaultSheetBuilder = new SheetBuilder(new CellTypeValueConverterFactory())
                .Decorate(new HeaderSheetDecorator())
                .Decorate(new AutoFilterSheetDecorator())
                .Decorate(new TextStyleSheetDecorator("Normal"))
                .Decorate(new HeaderStyleSheetDecorator("Output"))
                .Decorate(new AutoFitWidthSheetDecorator());

            var builder = new TemplatedExcelBuilder();

            var parser = new ExcelParser(cellTypeValueConverterFactory);

            Initialize(builder, parser);
        }

        internal TsdvReportLoader(IExcelBuilder builder, IExcelParser parser)
        {
            Initialize(builder, parser);
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
            _builder.DefineSheet<IBlockPlan>("BlockPlans", _defaultSheetBuilder).AddRange(BlockPlans);
            _builder.DefineSheet<IBlockPlanSetting>("BlockPlanSettings", _defaultSheetBuilder).AddRange(BlockPlanSettings);
            _builder.DefineSheet<ICustomTier>("CustomTiers", _defaultSheetBuilder).AddRange(CustomTiers);
            _builder.DefineSheet<ITierField>("TierFields", _defaultSheetBuilder).AddRange(TierFields);
            _builder.DefineSheet<ITierForm>("TierForms", _defaultSheetBuilder).AddRange(TierForms);
            _builder.DefineSheet<ITierFolder>("TierFolders", _defaultSheetBuilder).AddRange(TierFolders);
            _builder.DefineSheet<IExcludedStatus>("ExcludedStatuses", _defaultSheetBuilder).AddRange(ExcludedStatuses);
            _builder.DefineSheet<IRule>("Rules", _defaultSheetBuilder).AddRange(Rules);

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
    }
}