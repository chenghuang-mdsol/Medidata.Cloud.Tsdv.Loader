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
        private readonly ISheetBuilder _defaultSheetBuilder;
        private readonly IEnumerable<ISheetDefinition> _sheetDefinitions;
        private IExcelBuilder _builder;
        private IExcelParser _parser;

        public TsdvReportLoader(ICellTypeValueConverterFactory cellTypeValueConverterFactory, ILocalization localization)
        {
            _defaultSheetBuilder = new SheetBuilder(cellTypeValueConverterFactory)
                .Decorate(new HeaderSheetDecorator())
                .Decorate(new AutoFilterSheetDecorator())
                .Decorate(new TextStyleSheetDecorator("Normal"))
                .Decorate(new HeaderStyleSheetDecorator("Output"))
                .Decorate(new TranslateHeaderDecorator(localization))
                .Decorate(new AutoFitWidthSheetDecorator());

            var builder = new TemplatedExcelBuilder();

            var parser = new ExcelParser(cellTypeValueConverterFactory);

            _sheetDefinitions = new[]
                                {
                                    SheetDefinition.Define<BlockPlan>(),
                                    SheetDefinition.Define<BlockPlanSetting>(),
//                                        new SheetDefinition("BlockPlanSettings").DefineColumns<IBlockPlanSetting>(),
//                                        new SheetDefinition("CustomTiers").DefineColumns<ICustomTier>(),
//                                        new SheetDefinition("TierFields").DefineColumns<ITierField>(),
//                                        new SheetDefinition("TierForms").DefineColumns<ITierForm>(),
                                    SheetDefinition.Define<TierFolder>()
//                                        new SheetDefinition("ExcludedStatuses").DefineColumns<IExcludedStatus>(),
//                                        new SheetDefinition("Rules").DefineColumns<IRule>()
                                };


            Initialize(builder, parser);
        }

        public IList<BlockPlan> BlockPlans { get; private set; }
        public IList<BlockPlanSetting> BlockPlanSettings { get; private set; }
        public IList<ICustomTier> CustomTiers { get; private set; }
        public IList<ITierField> TierFields { get; private set; }
        public IList<ITierForm> TierForms { get; private set; }
        public IList<TierFolder> TierFolders { get; private set; }
        public IList<IExcludedStatus> ExcludedStatuses { get; private set; }
        public IList<IRule> Rules { get; private set; }

        public void Save(Stream outStream)
        {
            _builder.DefineSheet(GetSheetDefinition("BlockPlans"), _defaultSheetBuilder).AddRange(BlockPlans);
            _builder.DefineSheet(GetSheetDefinition("BlockPlanSettings"), _defaultSheetBuilder)
                    .AddRange(BlockPlanSettings);
//            _builder.DefineSheet(GetSheetDefinition("CustomTiers"), _defaultSheetBuilder).AddRange(CustomTiers);
//            _builder.DefineSheet(GetSheetDefinition("TierFields"), _defaultSheetBuilder).AddRange(TierFields);
//            _builder.DefineSheet(GetSheetDefinition("TierForms"), _defaultSheetBuilder).AddRange(TierForms);
            _builder.DefineSheet(GetSheetDefinition("TierFolders"), _defaultSheetBuilder).AddRange(TierFolders);
//            _builder.DefineSheet(GetSheetDefinition("ExcludedStatuses"), _defaultSheetBuilder).AddRange(ExcludedStatuses);
//            _builder.DefineSheet(GetSheetDefinition("Rules"), _defaultSheetBuilder).AddRange(Rules);

            _builder.Save(outStream);
        }

        public void Load(Stream source)
        {
            _parser.Load(source);

            BlockPlans = _parser.GetObjects<BlockPlan>(GetSheetDefinition("BlockPlans")).ToList();
            BlockPlanSettings = _parser.GetObjects<BlockPlanSetting>(GetSheetDefinition("BlockPlanSettings")).ToList();
//            CustomTiers = _parser.GetObjects<ICustomTier>(GetSheetDefinition("CustomTiers")).ToList();
//            TierFields = _parser.GetObjects<ITierField>(GetSheetDefinition("TierFields")).ToList();
//            TierForms = _parser.GetObjects<ITierForm>(GetSheetDefinition("TierForms")).ToList();
            TierFolders = _parser.GetObjects<TierFolder>(GetSheetDefinition("TierFolders")).ToList();
//            ExcludedStatuses = _parser.GetObjects<IExcludedStatus>(GetSheetDefinition("ExcludedStatuses")).ToList();
//            Rules = _parser.GetObjects<IRule>(GetSheetDefinition("Rules")).ToList();
        }

        public ISheetDefinition GetSheetDefinition(string name)
        {
            return _sheetDefinitions.First(x => x.Name == name);
        }

        private void Initialize(IExcelBuilder builder, IExcelParser parser)
        {
            _builder = builder;
            _parser = parser;
            BlockPlans = new List<BlockPlan>();
            BlockPlanSettings = new List<BlockPlanSetting>();
            CustomTiers = new List<ICustomTier>();
            TierFields = new List<ITierField>();
            TierForms = new List<ITierForm>();
            TierFolders = new List<TierFolder>();
            ExcludedStatuses = new List<IExcludedStatus>();
            Rules = new List<IRule>();
        }
    }
}