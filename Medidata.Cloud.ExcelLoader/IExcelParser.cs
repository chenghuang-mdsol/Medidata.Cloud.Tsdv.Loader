using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Medidata.Cloud.ExcelLoader
{
    public interface IExcelParser : IDisposable
    {
        IEnumerable<ExpandoObject> GetObjects(ISheetDefinition sheetDefinition, ISheetParser sheetParser);
        void Load(Stream stream);
    }
}