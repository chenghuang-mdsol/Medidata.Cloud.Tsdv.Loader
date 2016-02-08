using System.Collections;
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
            var context = _fixture.Create<IDictionary<string, object>>();
            var shouldContinue = _fixture.Create<bool>();
            var outMessage = _fixture.CreateMany<IValidationMessage>().ToList();
            _sut.Stub(x => x.Validate(Arg<IExcelLoader>.Is.Same(loader),
                                     Arg<IDictionary<string, object>>.Is.Same(context),
                                     out Arg<bool>.Out(shouldContinue).Dummy))
                .Return(outMessage);

            var result = _sut.Check(loader, context);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Messages);
            Assert.AreEqual(shouldContinue, result.ShouldContinue);
            CollectionAssert.AreEquivalent(result.Messages as ICollection, outMessage);
        }
    }
}