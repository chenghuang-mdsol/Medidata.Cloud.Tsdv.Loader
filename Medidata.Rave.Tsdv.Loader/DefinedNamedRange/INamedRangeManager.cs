using System.Collections.Generic;

namespace Medidata.Rave.Tsdv.Loader.DefinedNamedRange
{
    public interface INamedRangeManager
    {
        List<NamedRange> Resources { get; set; }
    }
}