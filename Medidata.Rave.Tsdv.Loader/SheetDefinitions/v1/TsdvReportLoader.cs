using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public class SheetObjectCollection<T> : List<T> where T: class
    {
        private ISheetDefinition _sheetDefinition;

        public SheetObjectCollection(string sheetName, IEnumerable<string> headers)
        {
            _sheetDefinition = typeof(T).GetSheetDefinitionFromType(sheetName, headers);
        }
    }
    public class TsdvReportLoader : ITsdvReportLoader
    {
        private readonly ISheetBuilder _defaultSheetBuilder;
        private readonly ISheetDefinition[] _sheetDefinitions;
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
                typeof(IBlockPlan).GetSheetDefinitionFromType("BlockPlans", new[] {"tsdv_header1"}),
                typeof(IBlockPlanSetting).GetSheetDefinitionFromType("BlockPlanSettings"),
                typeof(ICustomTier).GetSheetDefinitionFromType("CustomTiers"),
                typeof(ITierField).GetSheetDefinitionFromType("TierFields"),
                typeof(ITierForm).GetSheetDefinitionFromType("Rules"),
                typeof(ITierFolder).GetSheetDefinitionFromType("TierForms"),
                typeof(IExcludedStatus).GetSheetDefinitionFromType("TierFolders"),
                typeof(IRule).GetSheetDefinitionFromType("ExcludedStatuses")
            };

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
            _builder.DefineSheet(GetDef("BlockPlans"), _defaultSheetBuilder).AddRange(BlockPlans);
            _builder.DefineSheet(GetDef("BlockPlanSettings"), _defaultSheetBuilder).AddRange(BlockPlanSettings);
            _builder.DefineSheet(GetDef("CustomTiers"), _defaultSheetBuilder).AddRange(CustomTiers);
            _builder.DefineSheet(GetDef("TierFields"), _defaultSheetBuilder).AddRange(TierFields);
            _builder.DefineSheet(GetDef("TierForms"), _defaultSheetBuilder).AddRange(TierForms);
            _builder.DefineSheet(GetDef("TierFolders"), _defaultSheetBuilder).AddRange(TierFolders);
            _builder.DefineSheet(GetDef("ExcludedStatuses"), _defaultSheetBuilder).AddRange(ExcludedStatuses);
            _builder.DefineSheet(GetDef("Rules"), _defaultSheetBuilder).AddRange(Rules);

            _builder.Save(outStream);
        }

        public void Load(Stream source)
        {
            _parser.Load(source);

            BlockPlans = _parser.GetObjects<IBlockPlan>(GetDef("BlockPlans")).ToList();
            BlockPlanSettings = _parser.GetObjects<IBlockPlanSetting>(GetDef("BlockPlanSettings")).ToList();
            CustomTiers = _parser.GetObjects<ICustomTier>(GetDef("CustomTiers")).ToList();
            TierFields = _parser.GetObjects<ITierField>(GetDef("TierFields")).ToList();
            TierForms = _parser.GetObjects<ITierForm>(GetDef("TierForms")).ToList();
            TierFolders = _parser.GetObjects<ITierFolder>(GetDef("TierFolders")).ToList();
            ExcludedStatuses = _parser.GetObjects<IExcludedStatus>(GetDef("ExcludedStatuses")).ToList();
            Rules = _parser.GetObjects<IRule>(GetDef("Rules")).ToList();
        }

        private ISheetDefinition GetDef(string name)
        {
            return _sheetDefinitions.First(x => x.Name == name);
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