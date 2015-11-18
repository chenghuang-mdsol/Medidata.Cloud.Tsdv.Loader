using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medidata.Cloud.Tsdv.Loader.Attributes;
using Medidata.Cloud.Tsdv.Loader.Converters;

namespace Medidata.Cloud.Tsdv.Loader.ViewModels
{
    public class TSDV
    {
        [ExcelSheet(ConverterType = typeof(BlockPlanConverter))]
        public List<BlockPlan> BlockPlans { get; set; }
    }


}
