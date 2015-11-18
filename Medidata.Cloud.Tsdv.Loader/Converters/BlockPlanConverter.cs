using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using Medidata.Cloud.Tsdv.Loader.Attributes;
using Medidata.Cloud.Tsdv.Loader.Extensions;
using Medidata.Cloud.Tsdv.Loader.Localization;
using Medidata.Cloud.Tsdv.Loader.ViewModels;
using Medidata.Interfaces.Localization;


namespace Medidata.Cloud.Tsdv.Loader.Converters
{
        //public string Name { get; set; }
        //public string BlockPlanType { get; set; }
        //public string ObjectName { get; set; }
        //public bool IsProdInUse { get; set; }
        //public string RoleName { get; set; }
        //public bool Activated { get; set; }
        //public string ActivatedUserName { get; set; }
        //public decimal AverageSubjectPerSite { get; set; }
        //public decimal CoveragePercent { get; set; }
        //public string MatrixName { get; set; }
        //public DateTime? DateEstimated { get; set; }



    public class BlockPlanConverter :IConverter
    {
        //TODO: Replace the hardcoded EchoLocalizer
        public ILocalization _localization = new EchoLocalizer();
        public object ConvertBack(MiddleData data)
        {
            var rowData = data.RowData;
            var names = data.ColumnNames.Select(o=>o.RealName).ToList();
            return new BlockPlan()
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
            return _localization.GetLocalString(key, lang);
        }

        private ColumnName GetColumn(object obj, string lang, string propertyName)
        {
            return new ColumnName(GetLocalName(obj, lang, propertyName),propertyName, GetKey(obj, propertyName));
        }
        public MiddleData Convert(object obj)
        {
            var culture = CultureInfo.CurrentCulture;
            string lang = culture.ThreeLetterISOLanguageName;
            
            IList<ColumnName> columnData = new List<ColumnName>()
            {

                GetColumn(obj, lang,"Name"),
                GetColumn(obj, lang,"BlockPlanType"),
                GetColumn(obj, lang,"ObjectName"),
                GetColumn(obj, lang,"IsProdInUse"),
                GetColumn(obj, lang,"RoleName"),
                GetColumn(obj, lang,"Activated"),
                GetColumn(obj, lang,"ActivatedUserName"),
                GetColumn(obj, lang,"AverageSubjectPerSite"),
                GetColumn(obj, lang,"CoveragePercent"),
                GetColumn(obj, lang,"MatrixName"),
                GetColumn(obj, lang,"DateEstimated"),
            };
            if (!(obj is BlockPlan))
            {
                return null;
            }
            BlockPlan blockPlan = (BlockPlan)obj;
            IList<string> rowData = new List<string>()
            {
                blockPlan.Name,
                blockPlan.BlockPlanType,
                blockPlan.ObjectName,
                blockPlan.IsProdInUse ? "Yes" : "No",
                blockPlan.RoleName,
                blockPlan.Activated ? "Active" : "InActive",
                blockPlan.ActivatedUserName,
                blockPlan.AverageSubjectPerSite.ToString(),
                blockPlan.CoveragePercent.ToString(),
                blockPlan.MatrixName,
                blockPlan.DateEstimated.ToString()
            };
            //TODO: Add RealNames
            return new MiddleData(columnData,rowData);
        }
    }
}
