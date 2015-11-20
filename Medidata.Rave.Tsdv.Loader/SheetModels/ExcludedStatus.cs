using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
{
    public class ExcludedStatus : IExcludedStatus
    {
        public string SubjectStatus { get; set; }
        public bool Excluded { get; set; }
    }
}