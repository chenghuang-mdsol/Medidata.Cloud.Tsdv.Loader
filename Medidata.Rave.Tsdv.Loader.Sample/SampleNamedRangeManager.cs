using System.Collections.Generic;
using Medidata.Rave.Tsdv.Loader.DefinedNamedRange;

namespace Medidata.Rave.Tsdv.Loader.Sample
{
    public class NamedRangeManager : INamedRangeManager
    {
        private List<NamedRange> _resources;

        public IList<NamedRange> GetNamedRanges()
        {
            if (_resources != null) return _resources;
            return _resources = new List<NamedRange>
                        {
                            new NamedRange
                            {
                                Name = "Forms",
                                DependingKey = null,
                                Items = new List<NamedRangeItem>
                                        {
                                            new NamedRangeItem {NamedRangeName = "FormOidSource", Value = "VISIT"},
                                            new NamedRangeItem {NamedRangeName = "FormOidSource", Value = "SOMEDATE"},
                                            new NamedRangeItem {NamedRangeName = "FormOidSource", Value = "UNSCHEDULED"}
                                        }
                            },
                            new NamedRange
                            {
                                Name = "FormFields",
                                DependingKey = null,
                                Items = new List<NamedRangeItem>
                                        {
                                            new NamedRangeItem {NamedRangeName = "FieldOid.VISIT", Value = "Visit1"},
                                            new NamedRangeItem
                                            {
                                                NamedRangeName = "FieldOid.SOMEDATE",
                                                Value = "SomeDate"
                                            },
                                            new NamedRangeItem
                                            {
                                                NamedRangeName = "FieldOid.UNSCHEDULED",
                                                Value = "Unscheduled"
                                            },
                                            new NamedRangeItem {NamedRangeName = "FieldOid.VISIT", Value = "Visit2"},
                                            new NamedRangeItem {NamedRangeName = "FieldOid.VISIT", Value = "Visit3"},
                                            new NamedRangeItem
                                            {
                                                NamedRangeName = "FieldOid.SOMEDATE",
                                                Value = "SomeDate2"
                                            },
                                            new NamedRangeItem
                                            {
                                                NamedRangeName = "FieldOid.SOMEDATE",
                                                Value = "SomeDate3"
                                            }
                                        }
                            }
                        };
        }

    }
}