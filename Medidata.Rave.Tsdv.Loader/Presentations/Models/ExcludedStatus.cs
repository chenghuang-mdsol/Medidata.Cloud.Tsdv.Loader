using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
{
    public class ExcludedStatus : IExcludedStatus
    {
        public string SubjectStatus { get; set; }
        public bool Excluded { get; set; }
    }
}