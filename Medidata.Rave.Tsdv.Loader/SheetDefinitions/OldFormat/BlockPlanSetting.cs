using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.OldFormat
{
    [SheetName("BlockPlanSettings")]
    public class BlockPlanSetting : SheetModel
    {
        [ColumnHeaderName("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }

        [ColumnHeaderName("tsdv_Blocks")]
        public string Blocks { get; set; }

        [ColumnHeaderName("tsdv_BlockSubjectCount")]
        public int BlockSubjectCount { get; set; }

        [ColumnHeaderName("tsdv_Repeated")]
        public bool Repeated { get; set; }
    }
}