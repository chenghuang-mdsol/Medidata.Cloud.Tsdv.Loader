using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("ExcludedStatuses")]
    public class ExcludedStatus : SheetModel
    {
        [ColumnInfo("tsdv_StatusName")]
        public string SubjectStatus { get; set; }

        [ColumnInfo("tsdv_ExcludedURLProject")]
        public bool Excluded { get; set; }
    }
}