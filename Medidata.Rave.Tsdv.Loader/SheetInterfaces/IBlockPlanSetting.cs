namespace Medidata.Rave.Tsdv.Loader.SheetInterfaces
{
    public interface IBlockPlanSetting
    {
        string BlockPlanName { get; }
        string Blocks { get; }
        int BlockSubjectCound { get; }
        bool Repeated { get; }
    }
}