namespace Medidata.Rave.Tsdv.Loader.Presentations.Interfaces
{
    public interface IBlockPlanSetting
    {
        string BlockPlanName { get; }
        string Blocks { get; }
        int BlockSubjectCount { get; }
        bool Repeated { get; }
    }
}