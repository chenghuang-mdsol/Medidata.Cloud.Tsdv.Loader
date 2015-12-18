using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader;
using OfficeOpenXml;

namespace Medidata.Rave.Tsdv.Loader
{
    public class TemplatedExcelLoader : ExcelLoader
    {
        public TemplatedExcelLoader()
        {
            var ms = new MemoryStream(Resource.CoverSheet);
            Load(ms);
        }
    }
}