using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDecorators;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Medidata.Interfaces.Localization;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions
{
    public class TsdvReportLoader : ITsdvReportLoader
    {
        private readonly ISheetBuilder _defaultSheetBuilder;
        private readonly IEnumerable<ISheetDefinition> _sheetDefinitions;

        //        private readonly IDictionary<Type, ISheetDefinition> _sheetDefDic = new Dictionary<Type, ISheetDefinition>();
        //        private readonly IDictionary<Type, IList> _sheetDataDic = new Dictionary<Type, IList>();
        //        private readonly IDictionary<Type, IList<ExpandoObject>> _sheetLoadedDataDic = new Dictionary<Type, IList<ExpandoObject>>();

        private readonly IDictionary<Type, SheetInfo> _sheetInfoDic = new Dictionary<Type, SheetInfo>();
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
                                    SheetDefinition.Define<CustomTier>(),
                                    SheetDefinition.Define<TierField>(),
                                    SheetDefinition.Define<TierForm>(),
                                    SheetDefinition.Define<TierFolder>(),
                                    SheetDefinition.Define<ExcludedStatus>(),
                                    SheetDefinition.Define<Rule>()
                                };


            Initialize(builder, parser);
        }


        //        public IList<BlockPlan> BlockPlans { get; private set; }
        //        public IList<BlockPlanSetting> BlockPlanSettings { get; private set; }
        //        public IList<CustomTier> CustomTiers { get; private set; }
        //        public IList<TierField> TierFields { get; private set; }
        //        public IList<TierForm> TierForms { get; private set; }
        //        public IList<TierFolder> TierFolders { get; private set; }
        //        public IList<ExcludedStatus> ExcludedStatuses { get; private set; }
        //        public IList<Rule> Rules { get; private set; }

        public void Save(Stream outStream)
        {
            foreach (var type in _sheetInfoDic.Keys)
            {
                var info = _sheetInfoDic[type];
                var sheetDef = info.SheetDefinition;
                var sheetData = info.DataForSave;
                _builder.DefineSheet(sheetDef, _defaultSheetBuilder).AddRange(sheetData.Cast<SheetModel>());
            }

            _builder.Save(outStream);
        }

        public void Load(Stream source)
        {
            _parser.Load(source);

            //            BlockPlans = _parser.GetObjects<BlockPlan>(GetSheetDefinition("BlockPlans")).ToList();
            //            BlockPlanSettings = _parser.GetObjects<BlockPlanSetting>(GetSheetDefinition("BlockPlanSettings")).ToList();
            //            CustomTiers = _parser.GetObjects<CustomTier>(GetSheetDefinition("CustomTiers")).ToList();
            //            TierFields = _parser.GetObjects<TierField>(GetSheetDefinition("TierFields")).ToList();
            //            TierForms = _parser.GetObjects<TierForm>(GetSheetDefinition("TierForms")).ToList();
            //            TierFolders = _parser.GetObjects<TierFolder>(GetSheetDefinition("TierFolders")).ToList();
            //            ExcludedStatuses = _parser.GetObjects<ExcludedStatus>(GetSheetDefinition("ExcludedStatuses")).ToList();
            //            Rules = _parser.GetObjects<Rule>(GetSheetDefinition("Rules")).ToList();

            foreach (var type in _sheetInfoDic.Keys)
            {
                var info = _sheetInfoDic[type];
                var sheetDef = info.SheetDefinition;
                info.LoadedData = _parser.GetObjects(sheetDef).ToList();
            }
        }

        public ISheetDefinition AddOrGetSheetDefinition<T>() where T : SheetModel
        {
            SheetInfo info;
            if (_sheetInfoDic.TryGetValue(typeof(T), out info))
            {
                return info.SheetDefinition;
            }
            var sheetDef = SheetDefinition.Define<T>();
            _sheetInfoDic.Add(typeof(T), new SheetInfo {SheetDefinition = sheetDef});
            return sheetDef;
        }

        public IList<T> SheetData<T>() where T : SheetModel
        {
            var type = typeof(T);

            AddOrGetSheetDefinition<T>();

            var info = _sheetInfoDic[type];
            if (info.LoadedData != null)
            {
                info.DataForSave = info.LoadedData.OfSheetModel<T>().ToList();
            }
            else if (info.DataForSave == null)
            {
                info.DataForSave = new List<T>();
            }
            return (IList<T>) info.DataForSave;
        }

        //        public ISheetDefinition GetSheetDefinition(string name)
        //        {
        //            return _sheetDefinitions.First(x => x.Name == name);
        //        }

        private void Initialize(IExcelBuilder builder, IExcelParser parser)
        {
            _builder = builder;
            _parser = parser;
            //            BlockPlans = new List<BlockPlan>();
            //            BlockPlanSettings = new List<BlockPlanSetting>();
            //            CustomTiers = new List<CustomTier>();
            //            TierFields = new List<TierField>();
            //            TierForms = new List<TierForm>();
            //            TierFolders = new List<TierFolder>();
            //            ExcludedStatuses = new List<ExcludedStatus>();
            //            Rules = new List<Rule>();
        }

        private class SheetInfo
        {
            public ISheetDefinition SheetDefinition { get; set; }
            public IList DataForSave { get; set; }
            public IList<ExpandoObject> LoadedData { get; set; }
        }
    }
}