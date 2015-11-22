namespace Medidata.Rave.Tsdv.Loader.SheetSharps
{
    public interface IBlockPlanSetting
    {
        string BlockPlanName { get; }
        string Blocks { get; }
        int BlockSubjectCound { get; }
        bool Repeated { get; }
    }
}