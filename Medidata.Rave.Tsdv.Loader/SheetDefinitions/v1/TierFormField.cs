﻿using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.v1
{
    [ExcludeFromCodeCoverage]
    [SheetName("TierFormFields")]
    public class TierFormField : SheetModel
    {
        [ColumnInfo("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnInfo("FormOid", "FormOIDSource")]
        public string FormOid { get; set; }

        [ColumnInfo("FieldOid", "FieldOid.FormOid")]
        public string FieldOid { get; set; }

        [ColumnInfo("tsdv_Selected")]
        public bool Selected { get; set; }
    }
}