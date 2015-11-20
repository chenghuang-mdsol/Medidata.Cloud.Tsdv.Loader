using Medidata.Rave.Tsdv.Loader.SheetInterfaces;

namespace Medidata.Rave.Tsdv.Loader.SheetModels
{
    public class Rule : IRule
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Step { get; set; }
        public string Action { get; set; }
        public bool RunsRetrospective { get; set; }
        public int BackfillOpenSlots { get; set; }
        public string BlockPlanName { get; set; }
    }
}