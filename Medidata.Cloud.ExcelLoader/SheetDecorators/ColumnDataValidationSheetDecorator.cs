using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Medidata.Cloud.ExcelLoader.Helpers;

namespace Medidata.Cloud.ExcelLoader.SheetDecorators
{
    public class ColumnDataValidationSheetDecorator : ISheetBuilderDecorator
    {
        public ISheetBuilder Decorate(ISheetBuilder target)
        {
            var originalFunc = target.BuildSheet;
            target.BuildSheet = (models, sheetDefinition, doc) =>
            {
                originalFunc(models, sheetDefinition, doc);

                var validations = AddDataValidations(sheetDefinition.ColumnDefinitions);

                var worksheet = doc.GetWorksheetByName(sheetDefinition.Name);
                var node = worksheet.GetFirstChild<DataValidations>();
                if (!validations.Any()){ return; }
                    
                if (node == null)
                {
                    node = new DataValidations();
                    worksheet.Append(node);
                }
                node.Append(validations);
            };
            return target;
        }


        public virtual IEnumerable<DataValidation> AddDataValidations(IEnumerable<IColumnDefinition> columnDefinitions)
        {
            var defs = columnDefinitions.ToList();
            //Last row of excel
            const int lastRow = 1048576;

            //=FormOIDSource
            const string formula1 = "={0}";
            
            //=INDIRECT("FormOid.Form2")
            const string formula2 = "=INDIRECT(\"{0}.\" & OFFSET(INDIRECT(ADDRESS(ROW(), COLUMN())),0,{1}))";


            List<DataValidation> validations = new List<DataValidation>();
            for (int i = 0; i < defs.Count; i++)
            {
                var def = defs[i];
                
                string columnName = (i + 1).ConvertToColumnName();

                //without header: "A1:A1048576", with header : "A2:A1048576"
                int firstRow = def.Header == null ? 1 : 2;
                
                string source = def.ColumnSource;
                if (string.IsNullOrEmpty(source))
                {
                    continue;
                }
                string formula = null;
                var sourceParts = source.Split('.');
                if (sourceParts.Length == 2)
                {
                    //indirect source
                    //"{Header}.{DependentHeader}"
                    //"FieldOid.FormOid"
                    var indirectOnDef = defs.FirstOrDefault(e => e.Header == sourceParts[1]);
                    if (indirectOnDef != null)
                    {
                        var j = defs.IndexOf(indirectOnDef);
                        var offset = j - i;
                        formula = string.Format(formula2, def.Header, offset);
                    }
                }
                else
                {
                    //direct source
                    //"{Source}"
                    //"FormOIDSource"
                    formula = string.Format(formula1, sourceParts[0]);
                }

                var validation = new DataValidation
                {
                    Type = DataValidationValues.List,
                    AllowBlank = true,
                    ShowInputMessage = true,
                    ShowErrorMessage = true,
                    Formula1 = new Formula1(formula),
                    SequenceOfReferences =
                        new ListValue<StringValue>()
                        {
                            InnerText = string.Format("{0}{1}:{0}{2}", columnName, firstRow, lastRow)
                        }
                };

                validations.Add(validation);

            }
            return validations;
        }

    }

}
