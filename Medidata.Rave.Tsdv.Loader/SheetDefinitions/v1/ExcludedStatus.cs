using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("ExcludedStatuses")]
    public class ExcludedStatus : SheetModel
    {
        public string SubjectStatus { get; set; }
        public bool Excluded { get; set; }
    }
}