using System.Collections.Generic;
using Medidata.Cloud.ExcelLoader.DefinedNamedRange;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    public class NamedRangeManager : INamedRangeProvider
    {
        private List<NamedRange> _namedRanges;

        public IEnumerable<NamedRange> GetNamedRanges()
        {
            if (_namedRanges != null) return _namedRanges;
            return _namedRanges = new List<NamedRange>
                               {
                                   new NamedRange
                                   {
                                       ResourceName = "Forms",
                                       DependingKey = null,
                                       List = new List<NamedRangeItem>
                                              {
                                                  new NamedRangeItem {Category = "FormOidSource", Value = "VISIT"},
                                                  new NamedRangeItem {Category = "FormOidSource", Value = "SOMEDATE"},
                                                  new NamedRangeItem {Category = "FormOidSource", Value = "UNSCHEDULED"}
                                              }
                                   },
                                   new NamedRange
                                   {
                                       ResourceName = "FormFields",
                                       DependingKey = null,
                                       List = new List<NamedRangeItem>
                                              {
                                                  new NamedRangeItem {Category = "FieldOid.VISIT", Value = "Visit1"},
                                                  new NamedRangeItem
                                                  {
                                                      Category = "FieldOid.SOMEDATE",
                                                      Value = "SomeDate"
                                                  },
                                                  new NamedRangeItem
                                                  {
                                                      Category = "FieldOid.UNSCHEDULED",
                                                      Value = "Unscheduled"
                                                  },
                                                  new NamedRangeItem {Category = "FieldOid.VISIT", Value = "Visit2"},
                                                  new NamedRangeItem {Category = "FieldOid.VISIT", Value = "Visit3"},
                                                  new NamedRangeItem
                                                  {
                                                      Category = "FieldOid.SOMEDATE",
                                                      Value = "SomeDate2"
                                                  },
                                                  new NamedRangeItem
                                                  {
                                                      Category = "FieldOid.SOMEDATE",
                                                      Value = "SomeDate3"
                                                  }
                                              }
                                   }
                               };
        }
    }
}