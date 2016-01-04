using Medidata.Cloud.ExcelLoader;

namespace Medidata.Rave.Tsdv.Loader
{
    public interface ITsdvExcelLoaderFactory
    {
        IExcelLoader Create();
    }
}