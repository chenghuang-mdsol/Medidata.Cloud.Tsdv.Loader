using Medidata.Rave.Tsdv.Loader.Presentations.Interfaces;

namespace Medidata.Rave.Tsdv.Loader.Presentations.Models
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