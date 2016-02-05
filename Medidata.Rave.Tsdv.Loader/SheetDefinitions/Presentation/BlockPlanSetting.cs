using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("BlockPlanSettings")]
    public class BlockPlanSetting : SheetModel
    {
        [ColumnHeaderName("tsdv_BlockPlanName")]
        public string BlockPlanName { get; set; }

        [ColumnHeaderName("tsdv_Block")]
        public string Block { get; set; }

        [ColumnHeaderName("tsdv_BlockSubjectCount")]
        public int BlockSubjectCount { get; set; }

        [ColumnHeaderName("tsdv_Repeated")]
        public bool Repeated { get; set; }
    }
}