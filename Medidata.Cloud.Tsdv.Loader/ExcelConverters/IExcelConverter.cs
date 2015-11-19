using System;
using System.Text;

namespace Medidata.Cloud.Tsdv.Loader.ExcelConverters
{
    public interface IExcelConverter<T> : IExcelConverter
    {
        new T ConvertBack(MiddleData data);
        MiddleData Convert(T obj);
    }

    public interface IExcelConverter
    {
        object ConvertBack(MiddleData data);
        MiddleData Convert(object obj);
    }
}
