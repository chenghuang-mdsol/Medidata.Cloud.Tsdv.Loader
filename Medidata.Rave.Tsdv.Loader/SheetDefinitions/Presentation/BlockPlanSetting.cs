using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("BlockPlanSettings")]
    public class BlockPlanSetting : SheetModel
    {
        [ColumnInfo("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }

        [ColumnInfo("tsdv_Block")]
        public string Block { get; set; }

        [ColumnInfo("tsdv_BlockSubjectCount")]
        public int BlockSubjectCount { get; set; }

        [ColumnInfo("tsdv_Repeated")]
        public bool Repeated { get; set; }
    }
}