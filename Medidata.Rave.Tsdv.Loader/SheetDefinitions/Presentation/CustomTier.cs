﻿using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Rave.Tsdv.Loader.SheetDefinitions.Presentation
{
    [ExcludeFromCodeCoverage]
    [SheetName("CustomTiers")]
    public class CustomTier : SheetModel
    {
        [ColumnHeaderName("tsdv_TierName")]
        public string TierName { get; set; }

        [ColumnHeaderName("tsdv_TierDescription")]
        public string TierDescription { get; set; }

        [ColumnHeaderName("tsdv_LinkedToProdStudy")]
        public bool LinkedToProdStudy { get; set; }
    }
}