using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("Rules")]
    public class Rule: SheetModel
    {
      public  string Name { get; set; }
        public string Type { get; set; }
        public string Step { get; set; }
        public string Action { get; set; }
        public bool RunsRetrospective { get; set; }
        public int BackfillOpenSlots { get; set; }
        public string BlockPlanName { get; set; }
    }
}