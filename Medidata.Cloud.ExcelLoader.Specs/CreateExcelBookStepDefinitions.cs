using System;
using System.IO;
using System.Linq;
using Medidata.Cloud.ExcelLoader.SheetDefinitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Medidata.Cloud.ExcelLoader.Specs
{
    [Binding]
    public class CreateExcelBookStepDefinitions
    {
        private const string DefinedSheetName = "Tiny Sheet Name";
        private string _filePath;
        private IExcelLoader _loader;
        private TinySheetModel _rowModel;

        [Given(@"I have a sheet model class")]
        public void GivenIHaveASheetModelClass()
        {
            _loader = CreateExcelLoader();
        }

        private IExcelLoader CreateExcelLoader()
        {
            var converterManager = new CellTypeValueConverterManager();
            var excelBuilder = new ExcelBuilder();
            var excelParser = new ExcelParser();
            var sheetBuilder = new SheetBuilder(converterManager);
            var sheetParser = new SheetParser(converterManager);

            var loader = new ExcelLoader(excelBuilder, excelParser, sheetBuilder, sheetParser);
            loader.Sheet<TinySheetModel>();
            return loader;
        }

        [Given(@"I have a loader associated with a sheet model class")]
        public void GivenIHaveALoaderAssociatedWithASheetModelClass()
        {
            _loader = CreateExcelLoader();
        }

        [When(@"I add an instance of the sheet model")]
        public void WhenIAddAnInstanceOfTheSheetModel()
        {
            _rowModel = new TinySheetModel {Header1 = "MyStringValue", Header2 = 999};
            _loader.Sheet<TinySheetModel>().Data.Add(_rowModel);
        }


        [When(@"I save the workbook")]
        public void WhenISaveTheWorkbook()
        {
            _filePath = Path.GetTempFileName();
            using (var fs = new FileStream(_filePath, FileMode.Create))
            {
                _loader.Save(fs);
            }
        }

        private void ReadExcelAndAssert(Action<IExcelLoader> assertAction)
        {
            using (var fs = new FileStream(_filePath, FileMode.Open))
            {
                var reader = CreateExcelLoader();
                reader.Load(fs);
                assertAction(reader);
            }
        }

        [Then(@"there should be a worksheet with the name defined on the sheet model")]
        public void ThenThereShouldBeAWorksheetWithTheNameDefinedOnTheSheetModel()
        {
            ReadExcelAndAssert(loader =>
            {
                var sheet = loader.Sheet<TinySheetModel>();
                Assert.IsNotNull(sheet);
                Assert.IsNotNull(sheet.Definition);
                Assert.AreEqual(DefinedSheetName, sheet.Definition.Name);
            });
        }

        [Then(@"there should be a row that matches the sheet model")]
        public void ThenThereShouldBeARowThatMatchesTheSheetModel()
        {
            ReadExcelAndAssert(loader =>
            {
                var sheet = loader.Sheet<TinySheetModel>();
                Assert.IsNotNull(sheet);
                Assert.IsNotNull(sheet.Data);
                var row = sheet.Data.FirstOrDefault();
                Assert.IsNotNull(row);
                Assert.AreEqual(_rowModel.Header1, row.Header1);
                Assert.AreEqual(_rowModel.Header2, row.Header2);
            });
        }

        [SheetName(DefinedSheetName)]
        public class TinySheetModel : SheetModel
        {
            public string Header1 { get; set; }
            public int Header2 { get; set; }
        }
    }
}