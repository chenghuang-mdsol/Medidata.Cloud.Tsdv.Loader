using System.Collections.Generic;

namespace Medidata.Rave.Tsdv.Loader.DefinedNamedRange
{
    public class NamedRange
    {
        public string ResourceName { get; set; }
        public string DependingKey { get; set; }
        public bool IsDependent { get { return !string.IsNullOrEmpty(DependingKey); } }
        public List<NamedRangeItem> List { get; set; }
    }
}