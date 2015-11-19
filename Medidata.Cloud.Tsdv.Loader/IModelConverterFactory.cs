using System;
using Medidata.Cloud.Tsdv.Loader.ExcelConverters;

namespace Medidata.Cloud.Tsdv.Loader
{
    public interface IModelConverterFactory
    {
        IModelConverter ProduceConverter(Type type);
    }

    public interface IExcelConverterFactory
    {
        IExcelConverter ProduceConverter(Type type);
    }


}