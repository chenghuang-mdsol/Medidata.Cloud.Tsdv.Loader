using System.Collections.Generic;

namespace Medidata.Rave.Tsdv.Loader.ColumnResources
{
    public class ColumnResource
    {
        public string ResourceName { get; set; }
        public string DependingKey { get; set; }
        public bool IsDependent { get { return !string.IsNullOrEmpty(DependingKey); } }
        public List<ColumnResourceItem> List { get; set; }
    }
}