using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medidata.Cloud.ExcelLoader;
using Medidata.Interfaces.Localization;

namespace Medidata.Rave.Tsdv.Loader
{
    public class LocalizedExcelBuilder: ExcelBuilder
    {
        private readonly ILocalization _localization;

        public LocalizedExcelBuilder(ILocalization localization)
        {
            if (localization == null) throw new ArgumentNullException("localization");
            _localization = localization;
        }

        protected override string[] GetPropertyNames<T>(string[] columnNames)
        {
            return columnNames == null
                ? base.GetPropertyNames<T>(null) 
                : columnNames.Select(x => _localization.GetLocalString(x)).ToArray();
        }
    }
}
