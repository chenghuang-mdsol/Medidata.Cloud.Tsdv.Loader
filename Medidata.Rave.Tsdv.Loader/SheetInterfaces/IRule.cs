namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
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