namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    public interface IBlockPlanSetting
    {
        string BlockPlanName { get; }
        string Blocks { get; }
        int BlockSubjectCound { get; }
        bool Repeated { get; }
    }
}