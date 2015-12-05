using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medidata.Cloud.ExcelLoader
{
    public interface IDynamicFields
    {
        IList DynamicFields { get; set; }
        string[] DynamicColumnNames { get; set; }
    }
}
