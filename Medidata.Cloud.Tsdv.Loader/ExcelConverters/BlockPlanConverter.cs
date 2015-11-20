using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using ImpromptuInterface;
using Medidata.Cloud.Tsdv.Loader.Attributes;
using Medidata.Cloud.Tsdv.Loader.Converters;
using Medidata.Cloud.Tsdv.Loader.Extensions;
using Medidata.Cloud.Tsdv.Loader.Localization;
using Medidata.Cloud.Tsdv.Loader.Models;
using Medidata.Interfaces.Localization;
using Medidata.Interfaces.TSDV;


namespace Medidata.Cloud.Tsdv.Loader.ExcelConverters
{
    public class CustomizedConvert
    {
        public Dictionary<string, Func<object,object>> ConvertOverrideDict { get; set; }

        public CustomizedConvert()
        {
            ConvertOverrideDict = new Dictionary<string, Func<object, object>>();
        }
        public void Add(string propertyName, Func<object,object> func)
        {
            if (ConvertOverrideDict.ContainsKey(propertyName))
            {
                ConvertOverrideDict[propertyName] = func;
            }
            else
            {
                ConvertOverrideDict.Add(propertyName, func);
            }
        }

        public Func<object,object> GetFunc(string propertyName)
        {
            if (ConvertOverrideDict.ContainsKey(propertyName))
            {
                return ConvertOverrideDict[propertyName];
            }
            return null;
        }
    }

    public class MiddleDataConverterBase<TInterface, TModelType> : IExcelConverter 
        where TInterface: class
        where TModelType: new()
    {
        private readonly CustomizedConvert _customizedConvertBack =new CustomizedConvert();
        private readonly CustomizedConvert _customizedConvert = new CustomizedConvert();
        public Type InterfaceType { get; set; }
        

        public ILocalization Localization { get; set; }

        public MiddleDataConverterBase()
        {
            InterfaceType = typeof (TInterface);
        } 
        public void AddCustomConvertBack(string propertyName, Func<object,object> func)
        {
            _customizedConvertBack.Add(propertyName, func);
        }
        public void AddCustomConvert(string propertyName, Func<object, object> func)
        {
            _customizedConvert.Add(propertyName, func);
        }

        public object ConvertBack(MiddleData data)
        {
            var rowData = data.RowData;
            var names = data.ColumnNames.Select(o => o.PropertyName).ToList();
            TModelType result = new TModelType();
            var properties = typeof (TInterface).GetProperties();
            foreach (var pi in properties)
            {
                string value = rowData[names.IndexOf(pi.Name)];
                var func = _customizedConvertBack.GetFunc(pi.Name);
                if (func != null)
                {
                    var newValue = func(value);
                    pi.SetValue(result, newValue, null);
                }
                else
                {
                    pi.SetValue(result, value, null);
                }
            }
            return result;
        }

        public MiddleData Convert(object obj)
        {
            var culture = CultureInfo.CurrentCulture;
            string lang = culture.ThreeLetterISOLanguageName;

            IList<ColumnName> columnData = GetColumnData(obj, lang);
            if (!(obj is TModelType))
            {
                return null;
            }
            var properties = typeof(TModelType).GetProperties();
            List<string> rowData = new List<string>();
            foreach (var pi in properties)
            {
                var value = pi.GetValue(obj, null);
                var func = _customizedConvert.GetFunc(pi.Name);
                if (func != null)
                {
                    var newValue = func(value);
                    rowData.Add(newValue.ToString());
                }
                else
                {
                    rowData.Add(value.ToString());
                }
            }
            return new MiddleData(columnData, rowData);
        }

        private string GetKey(object obj, string propertyName)
        {
            return obj.GetAttributeValue<ExcelColumnAttribute, string>(propertyName, false, "LocalizationKey", propertyName);
        }

        private string GetLocalName(object obj, string lang, string propertyName)
        {
            var key = GetKey(obj, propertyName);
            return Localization.GetLocalString(key, lang);
        }

        private ColumnName GetColumn(object obj, string lang, string propertyName)
        {
            return new ColumnName(GetLocalName(obj, lang, propertyName), propertyName, GetKey(obj, propertyName));
        }

        private IList<ColumnName> GetColumnData(object obj, string lang)
        {
            return obj.GetAllPropertyNames().Select(o => GetColumn(obj, lang, o)).ToList();
        }


    }


    public class BlockPlanConverter2 : MiddleDataConverterBase<IBlockPlan,BlockPlanModel>
    {
        public BlockPlanConverter2()
        {
            Localization = new EchoLocalizer();

            AddCustomConvertBack("IsProdInUse", o=>((string)o).ToBoolean());
            AddCustomConvertBack("Activated", o => ((string)o).ToBoolean("Active"));
            AddCustomConvertBack("AverageSubjectPerSite", o => ((string)o).ToDecimal());
            AddCustomConvertBack("CoveragePercent", o => ((string)o).ToDecimal());
            AddCustomConvertBack("DateEstimated", o => ((string)o).ToDateTimeNullable());

            AddCustomConvert("IsProdInUse", o => o == null?"Yes":"No");
            AddCustomConvert("Activated", o => o == null ? "Active" : "InActive");
            AddCustomConvert("AverageSubjectPerSite", o => o.ToString());
            AddCustomConvert("DateEstimated", o =>  o.ToString());
        }
    }

    public class BlockPlanConverter :IExcelConverter
    {
        public Type InterfaceType { get; set; }
        public ILocalization Localization { get; set; } 

        public BlockPlanConverter()
        {
            //TODO: Replace the hardcoded EchoLocalizer
            Localization = new EchoLocalizer();
        }
        public object ConvertBack(MiddleData data)
        {
            var rowData = data.RowData;
            var names = data.ColumnNames.Select(o=>o.PropertyName).ToList();
            return new BlockPlanModel()
            {
                Name = rowData[names.IndexFor("Name")],
                BlockPlanType = rowData[names.IndexFor("BlockPlanType")],
                ObjectName = rowData[names.IndexFor("ObjectName")],
                IsProdInUse = rowData[names.IndexFor( "IsProdInUse")].ToBoolean(),
                RoleName = rowData[names.IndexFor("RoleName")],
                Activated = rowData[names.IndexFor("Activated")].ToBoolean("Active"),
                ActivatedUserName = rowData[names.IndexFor("ActivatedUserName")],
                AverageSubjectPerSite = rowData[names.IndexFor("AverageSubjectPerSite")].ToDecimal(),
                CoveragePercent = rowData[names.IndexFor("CoveragePercent")].ToDecimal(),
                MatrixName = rowData[names.IndexFor("MatrixName")],
                DateEstimated = rowData[names.IndexFor("DateEstimated")].ToDateTimeNullable()
            };
        }
        
        private string GetKey(object obj, string propertyName)
        {
            return obj.GetAttributeValue<ExcelColumnAttribute, string>(propertyName, false, "LocalizationKey", propertyName);
        }

        private string GetLocalName(object obj, string lang, string propertyName)
        {
            var key = GetKey(obj, propertyName);
            return Localization.GetLocalString(key, lang);
        }

        private ColumnName GetColumn(object obj, string lang, string propertyName)
        {
            return new ColumnName(GetLocalName(obj, lang, propertyName),propertyName, GetKey(obj, propertyName));
        }

        private IList<ColumnName> GetColumnData(object obj, string lang)
        {
            return obj.GetAllPropertyNames().Select(o => GetColumn(obj, lang, o)).ToList();
        }
        public MiddleData Convert(object obj)
        {
            var culture = CultureInfo.CurrentCulture;
            string lang = culture.ThreeLetterISOLanguageName;

            IList<ColumnName> columnData = GetColumnData(obj, lang);
            if (!(obj is BlockPlanModel))
            {
                return null;
            }
            BlockPlanModel blockPlanExcelModel = (BlockPlanModel)obj;
            IList<string> rowData = new List<string>()
            {
                blockPlanExcelModel.Name,
                blockPlanExcelModel.BlockPlanType,
                blockPlanExcelModel.ObjectName,
                blockPlanExcelModel.IsProdInUse ? "Yes" : "No",
                blockPlanExcelModel.RoleName,
                blockPlanExcelModel.Activated ? "Active" : "InActive",
                blockPlanExcelModel.ActivatedUserName,
                blockPlanExcelModel.AverageSubjectPerSite.ToString(),
                blockPlanExcelModel.CoveragePercent.ToString(),
                blockPlanExcelModel.MatrixName,
                blockPlanExcelModel.DateEstimated.ToString()
            };
            return new MiddleData(columnData,rowData);
        }
    }
}
