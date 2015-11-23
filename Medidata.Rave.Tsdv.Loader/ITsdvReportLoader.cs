using System.IO;

namespace Medidata.Rave.Tsdv.Loader
{
    public interface ITsdvReportLoader
    {
        void Save(Stream outStream);
        void Load(Stream source);
    }
}