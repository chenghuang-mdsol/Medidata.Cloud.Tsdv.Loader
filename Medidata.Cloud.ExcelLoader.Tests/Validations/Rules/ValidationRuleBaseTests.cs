using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Cloud.ExcelLoader.Validations.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Cloud.ExcelLoader.Tests.Validations.Rules
{
    [TestClass]
    public class ValidationRuleBaseTests
    {
        private IFixture _fixture;
        private ValidationRuleBase _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());

            _sut = MockRepository.GeneratePartialMock<ValidationRuleBase>();
        }

        [TestMethod]
        public void CheckFiresValidateMethod()
        {
            var loader = _fixture.Create<IExcelLoader>();
            var outMessage = _fixture.CreateMany<IValidationMessage>().ToList();
            _sut.Expect(
                x => x.Validate(Arg<IExcelLoader>.Is.Same(loader),
                    out Arg<IList<IValidationMessage>>.Out(outMessage).Dummy,
                    Arg<Action>.Is.Anything));

            var result = _sut.Check(loader);

            _sut.VerifyAllExpectations();
            Assert.AreEqual(result.Messages, outMessage);
        }
    }
}