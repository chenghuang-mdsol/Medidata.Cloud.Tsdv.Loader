using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("Rules")]
    public class Rule : SheetModel
    {
        [ColumnHeaderName("tsdv_Name")]
        public string Name { get; set; }

        [ColumnHeaderName("tsdv_Type")]
        public string Type { get; set; }

        [ColumnHeaderName("tsdv_Step")]
        public string Step { get; set; }

        [ColumnHeaderName("tsdv_Action")]
        public string Action { get; set; }

        [ColumnHeaderName("tsdv_RunsRetrospective")]
        public bool RunsRetrospective { get; set; }

        [ColumnHeaderName("tsdv_BackfillOpenSlots")]
        public int BackfillOpenSlots { get; set; }

        [ColumnHeaderName("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }
    }
}