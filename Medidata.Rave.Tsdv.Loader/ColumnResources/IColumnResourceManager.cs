using System.Collections.Generic;

namespace Medidata.Rave.Tsdv.Loader.ColumnResources
{
    public interface IColumnResourceManager
    {
        List<ColumnResource> Resources { get; set; }
    }
}