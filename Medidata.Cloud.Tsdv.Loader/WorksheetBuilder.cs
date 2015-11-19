using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.Tsdv.Loader.Helpers;

namespace Medidata.Cloud.Tsdv.Loader
{
    public class WorksheetBuilder<T> : IWorksheetBuilder<T> where T : class
    {
        private readonly IModelConverterFactory _modelConverterFactory;
        private readonly IExcelConverterFactory _excelConverterFactory;
        private readonly IList<T> _objects = new List<T>();
        private Type _objectType;

        public WorksheetBuilder(IModelConverterFactory modelConverterFactory, IExcelConverterFactory excelConverterFactory)
        {
            if (modelConverterFactory == null) throw new ArgumentNullException("modelConverterFactory");
            if (excelConverterFactory == null) throw new ArgumentNullException("excelConverterFactory");
            _objectType = typeof(T);
            _modelConverterFactory = modelConverterFactory;
            _excelConverterFactory = excelConverterFactory;
        }


        public void AddObject(T target)
        {
            _objects.Add(target);
        }

        public Worksheet ToWorksheet()
        {
            var converter = _modelConverterFactory.ProduceConverter(_objectType);
            var excelConverter = _excelConverterFactory.ProduceConverter(typeof (T));
            var models = _objects.Select(x => converter.ConvertToModel(x)).ToList();
            ExcelHelper helper = new ExcelHelper();
            Worksheet sheet = new Worksheet();
            sheet.AddNamespaceDeclaration("mdsol", "http://www.mdsol.com");
            var result = helper.ConvertToWorkSheet(models, excelConverter);
            sheet.AppendChild(result);
            return sheet;

        }
    }
}