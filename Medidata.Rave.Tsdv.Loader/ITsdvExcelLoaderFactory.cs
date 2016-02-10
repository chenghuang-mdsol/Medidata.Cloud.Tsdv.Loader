using Medidata.Cloud.ExcelLoader;
using Medidata.Rave.Tsdv.Loader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader
{
    public interface ITsdvExcelLoaderFactory
    {
        IExcelLoader Create(TsdvLoaderSupportedVersion version);
    }
}