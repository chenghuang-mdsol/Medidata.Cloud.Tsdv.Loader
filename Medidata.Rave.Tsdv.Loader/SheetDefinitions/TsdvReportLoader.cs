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
        private readonly IDictionary<Type, SheetInfo> _sheetInfoDic = new Dictionary<Type, SheetInfo>();
        private readonly IExcelBuilder _builder;
        private readonly IExcelParser _parser;


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

            _builder = builder;
            _parser = parser;

            SetupSheets();
        }

        private void SetupSheets()
        {
            AddOrGetSheetDefinition<BlockPlan>();
            AddOrGetSheetDefinition<BlockPlanSetting>();
            AddOrGetSheetDefinition<CustomTier>();
            AddOrGetSheetDefinition<TierField>();
            AddOrGetSheetDefinition<TierForm>();
            AddOrGetSheetDefinition<TierFolder>();
            AddOrGetSheetDefinition<ExcludedStatus>();
            AddOrGetSheetDefinition<Rule>();
        }

        public void Save(Stream outStream)
        {
            foreach (var type in _sheetInfoDic.Keys)
            {
                var info = _sheetInfoDic[type];
                var sheetDef = info.SheetDefinition;
                var sheetData = info.DataForSave ?? new List<SheetModel>();
                _builder.DefineSheet(sheetDef, _defaultSheetBuilder).AddRange(sheetData.Cast<SheetModel>());
            }

            _builder.Save(outStream);
        }

        public void Load(Stream source)
        {
            _parser.Load(source);

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

        private class SheetInfo
        {
            public ISheetDefinition SheetDefinition { get; set; }
            public IList DataForSave { get; set; }
            public IList<ExpandoObject> LoadedData { get; set; }
        }
    }
}