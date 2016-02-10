using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [ExcludeFromCodeCoverage]
    [SheetName("BlockPlanSettings")]
    public class BlockPlanSetting : SheetModel
    {
        [ColumnInfo("tsdv_Block")]
        public string Block { get; set; }

        [ColumnInfo("tsdv_BlockSubjectCount")]
        public int BlockSubjectCount { get; set; }

        [ColumnInfo("tsdv_Repeated")]
        public bool Repeated { get; set; }
    }
}