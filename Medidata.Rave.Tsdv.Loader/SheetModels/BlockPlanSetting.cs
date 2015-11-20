using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
{
    public class BlockPlanSetting : IBlockPlanSetting
    {
        public string BlockPlanName { get; set; }
        public string Blocks { get; set; }
        public int BlockSubjectCound { get; set; }
        public bool Repeated { get; set; }
    }
}