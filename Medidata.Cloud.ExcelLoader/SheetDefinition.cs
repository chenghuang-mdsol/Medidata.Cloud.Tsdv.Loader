using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader.Helpers;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Cloud.ExcelLoader
{
    public class SheetDefinition : ISheetDefinition
    {
        internal SheetDefinition() {}

        public string Name { get; set; }
        public int HeaderRowCount { get; set; }
        public IEnumerable<IColumnDefinition> ColumnDefinitions { get; set; }

        public ISheetDefinition AddColumn(IColumnDefinition columnDefinition)
        {
            if (columnDefinition == null) throw new ArgumentNullException("columnDefinition");
            if (ColumnDefinitions.All(x => x.PropertyName != columnDefinition.PropertyName))
            {
                ColumnDefinitions = ColumnDefinitions.Concat(new[] {columnDefinition});
            }
            return this;
        }

        public static ISheetDefinition Define<T>()
        {
            var type = typeof(T);
            var sheetNameAtt = type.GetCustomAttributes(false).OfType<SheetNameAttribute>().SingleOrDefault();
            if (sheetNameAtt == null)
                throw new Exception("Model type must have SheetNameAttribute");

            var props = type.GetPropertyDescriptors();
            var colDefinitions = from prop in props
                                 let headerAtt = prop.Attributes.OfType<ColumnInfoAttribute>().SingleOrDefault()
                                 select new ColumnDefinition
                                        {
                                            PropertyName = prop.Name,
                                            Header = headerAtt != null ? headerAtt.Header : prop.Name,
                                            ColumnSource = headerAtt != null ? headerAtt.Source : string.Empty,
                                            ExtraProperty = false
                                        };

            var sheetDefinition = new SheetDefinition
                                  {
                                      Name = sheetNameAtt.SheetName,
                                      ColumnDefinitions = colDefinitions
                                  };
            return sheetDefinition;
        }
    }
}