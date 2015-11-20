using System;
using System.Text;
using Medidata.Interfaces.Localization;

namespace Medidata.Cloud.Tsdv.Loader.ExcelConverters
{
    public interface IExcelConverter<T> : IExcelConverter
    {
        new T ConvertBack(MiddleData data);
        MiddleData Convert(T obj);
    }

    public interface IExcelConverter
    {
        Type InterfaceType { get; set; }
        //Type ModelType { get; set; }
        ILocalization Localization { get; set; }
        object ConvertBack(MiddleData data);
        MiddleData Convert(object obj);
        
    }
}
