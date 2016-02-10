using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;

namespace Medidata.Cloud.ExcelLoader
{
    public class ExcelBuilder : IExcelBuilder
    {
        private readonly IList<SheetInfo> _allSheetInfos = new List<SheetInfo>();

        public void AddSheet(ISheetDefinition sheetDefinition, IEnumerable<SheetModel> models, ISheetBuilder sheetBuilder)
        {
            if (sheetDefinition == null) throw new ArgumentNullException("sheetDefinition");
            if (models == null) throw new ArgumentNullException("models");
            if (sheetBuilder == null) throw new ArgumentNullException("sheetBuilder");
            var sheetInfo = new SheetInfo(models) {SheetDefinition = sheetDefinition, SheetBuilder = sheetBuilder};
            _allSheetInfos.Add(sheetInfo);
        }

        public virtual void Save(Stream outStream)
        {
            if (outStream == null) throw new ArgumentNullException("outStream");
            using (var doc = CreateDocument(outStream))
            {
                WorkbookPart workbookPart;
                if (doc.WorkbookPart == null)
                {
                    workbookPart = doc.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();
                }
                else
                {
                    workbookPart = doc.WorkbookPart;
                }

                foreach (var sheetInfo in _allSheetInfos)
                {
                    sheetInfo.SheetBuilder.BuildSheet(sheetInfo, sheetInfo.SheetDefinition, doc);
                }
                PostBuildSheets(doc);
                workbookPart.Workbook.Save();
            }
        }

        protected virtual SpreadsheetDocument CreateDocument(Stream outStream)
        {
            if (outStream == null) throw new ArgumentNullException("outStream");
            return SpreadsheetDocument.Create(outStream, SpreadsheetDocumentType.Workbook);
        }

        protected virtual void PostBuildSheets(SpreadsheetDocument doc)
        {
            
        }

        private class SheetInfo : List<SheetModel>
        {
            public SheetInfo(IEnumerable<SheetModel> models) : base(models) {}
            public ISheetDefinition SheetDefinition { get; set; }
            public ISheetBuilder SheetBuilder { get; set; }
        }
    }
}