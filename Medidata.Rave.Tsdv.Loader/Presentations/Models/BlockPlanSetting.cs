using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
{
    public class BlockPlanSetting : IBlockPlanSetting
    {
        public string BlockPlanName { get; set; }
        public string Blocks { get; set; }
        public int BlockSubjectCound { get; set; }
        public bool Repeated { get; set; }
    }
}