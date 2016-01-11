using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("ExcludedStatuses")]
    public class ExcludedStatus : SheetModel
    {
        [ColumnHeaderName("tsdv_StatusName")]
        public string SubjectStatus { get; set; }

        [ColumnHeaderName("tsdv_ExcludedURLProject")]
        public bool Excluded { get; set; }
    }
}