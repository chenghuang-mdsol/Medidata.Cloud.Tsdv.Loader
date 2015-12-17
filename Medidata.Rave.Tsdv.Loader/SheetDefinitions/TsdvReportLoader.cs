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
        private readonly ISheetBuilder _sheetBuilder;
        private readonly ISheetParser _sheetParser;
        private readonly IDictionary<Type, SheetInfo> _sheetInfoDic = new Dictionary<Type, SheetInfo>();
        private readonly IExcelBuilder _builder;
        private readonly IExcelParser _parser;


        public TsdvReportLoader(IExcelBuilder builder, IExcelParser parser, ISheetBuilder sheetBuilder, ISheetParser sheetParser)
        {
            _sheetBuilder = sheetBuilder;
            _sheetParser = sheetParser;

            _builder = builder;
            _parser = parser;

            SetupSheets();
        }

        private void SetupSheets()
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

        public void Save(Stream outStream)
        {
            foreach (var type in _sheetInfoDic.Keys)
            {
                var info = _sheetInfoDic[type];
                var sheetDef = info.SheetDefinition;
                var sheetData = info.DataForSave ?? new List<SheetModel>();
                _builder.AddSheet(sheetDef, sheetData.Cast<SheetModel>(), _sheetBuilder);
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
                info.LoadedData = _parser.GetObjects(sheetDef, _sheetParser).ToList();
            }
        }

        public ISheetDefinition SheetDefinition<T>() where T : SheetModel
        {
            SheetInfo info;
            if (_sheetInfoDic.TryGetValue(typeof(T), out info))
            {
                return info.SheetDefinition;
            }
            var sheetDef = Cloud.ExcelLoader.SheetDefinition.Define<T>();
            _sheetInfoDic.Add(typeof(T), new SheetInfo {SheetDefinition = sheetDef});
            return sheetDef;
        }

        public IList<T> SheetData<T>() where T : SheetModel
        {
            var type = typeof(T);

            SheetDefinition<T>();

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