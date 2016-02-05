using Medidata.Cloud.ExcelLoader.CellTypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.ExcelLoader.Tests.CellTypeConverters
{
    [TestClass]
    public class IntConverterTests
    {
        private IFixture _fixture;
        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
        }

        [TestMethod]
        public void ShouldConvertToCSharpValue()
        {
            //Arrange
            object value0;
            object value1;
            object value2;
            var converter = new IntConverter();
            //Act
            bool success0 = converter.TryConvertToCSharpValue("123", out value0);
            bool success1 = converter.TryConvertToCSharpValue("-1000", out value1);
            bool success2 = converter.TryConvertToCSharpValue("0", out value2);
            //Assert
            Assert.IsTrue(success0);
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
            Assert.AreEqual(123, value0);
            Assert.AreEqual(-1000, value1);
            Assert.AreEqual(0, value2);
        }



        [TestMethod]
        public void ShouldFailToConvertToCSharpValue()
        {
            //Arrange
            object value0;
            object value1;
            object value2;
            object value3;
            object value4;
            var converter = new IntConverter();
            //Act
            bool success0 = converter.TryConvertToCSharpValue("9E10D", out value0);
            bool success1 = converter.TryConvertToCSharpValue("N/A", out value1);
            bool success2 = converter.TryConvertToCSharpValue("NaN", out value2);
            bool success3 = converter.TryConvertToCSharpValue("", out value3);
            bool success4 = converter.TryConvertToCSharpValue(null, out value4);
            //Assert
            Assert.IsFalse(success0);
            Assert.IsFalse(success1);
            Assert.IsFalse(success2);
            Assert.IsFalse(success3);
            Assert.IsFalse(success4);
            Assert.IsNull(value0);
            Assert.IsNull(value1);
            Assert.IsNull(value2);
            Assert.IsNull(value3);
            Assert.IsNull(value4);
        }

        [TestMethod]
        public void ShouldConvertToCellValue()
        {
            //Arrange
            string value0;
            string value1;
            string value2;
            var converter = new IntConverter();

            //Act
            bool success0 = converter.TryConvertToCellValue(123, out value0);
            bool success1 = converter.TryConvertToCellValue(int.MaxValue, out value1);
            bool success2 = converter.TryConvertToCellValue(-15, out value2);

            //Assert
            Assert.IsTrue(success0);
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
            Assert.AreEqual("123", value0);
            Assert.AreEqual(int.MaxValue.ToString(), value1);
            Assert.AreEqual("-15", value2);
        }

        [TestMethod]
        public void ShouldFailToConvertToCellValue()
        {
            //Arrange
            string value0;
            string value1;
            string value2;
            var converter = new IntConverter();
            //Act
            bool success0 = converter.TryConvertToCellValue(0.18D, out value0);
            bool success1 = converter.TryConvertToCellValue("-1", out value1);
            bool success2 = converter.TryConvertToCellValue("NaN", out value2);
            //Assert
            Assert.IsFalse(success0);
            Assert.IsFalse(success1);
            Assert.IsFalse(success2);
        }


    }
}