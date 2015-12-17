using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [SheetName("BlockPlanSettings")]
    public class BlockPlanSetting : SheetDefinitionModelBase
    {
        public string BlockPlanName { get; set; }
        public string Blocks { get; set; }
        public int BlockSubjectCount { get; set; }
        public bool Repeated { get; set; }
    }
}