namespace Medidata.Rave.Tsdv.Loader.SheetSharps
{
    public interface IRule
    {
        string Name { get; }
        string Type { get; }
        string Step { get; }
        string Action { get; }
        bool RunsRetrospective { get; }
        int BackfillOpenSlots { get; }
        string BlockPlanName { get; }
    }
}