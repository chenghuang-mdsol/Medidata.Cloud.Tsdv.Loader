using System;
using System.Text;

namespace Medidata.Cloud.Tsdv.Loader.Converters
{
    public interface IConverter<T> : IConverter
    {
        new T ConvertBack(MiddleData data);
        MiddleData Convert(T obj);
    }

    public interface IConverter
    {
        object ConvertBack(MiddleData data);
        MiddleData Convert(object obj);
    }
}
