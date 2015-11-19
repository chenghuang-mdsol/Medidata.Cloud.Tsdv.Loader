using System;
using Medidata.Cloud.Tsdv.Loader.ModelConverters;
using Medidata.Cloud.Tsdv.Loader.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Tsdv.Loader.Tests
{
    [TestClass]
    public class PropNameMatchBaseConverterTests
    {
        private IFixture _fixture;
        private PropNameMatchBaseConverter<IFakeInterface, FakeModel> _sut;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _sut = new PropNameMatchBaseConverter<IFakeInterface, FakeModel>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Ctor_TInterfaceIsNotInterface_ArgumentException()
        {
            var sut = new PropNameMatchBaseConverter<FakeClass, FakeClass>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Ctor_TModelIsNotClass_ArgumentException()
        {
            var sut = new PropNameMatchBaseConverter<IFakeInterface, IFakeInterface>();
        }

        [TestMethod]
        public void Ctor_InterfaceTypeModelType()
        {
            var sut = new PropNameMatchBaseConverter<IFakeInterface, FakeClass>();

            Assert.AreEqual(typeof (IFakeInterface), sut.InterfaceType);
            Assert.AreEqual(typeof (FakeClass), sut.ModelType);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void ConvertToModel_NotImplementingInterface_InvalidCastException()
        {
            var target = new FakeNotImplClass();

            var model = _sut.ConvertToModel(target);
        }

        [TestMethod]
        public void ConvertToModel_ModelPropertyIsSet()
        {
            var target = new FakeClass {Name = _fixture.Create<string>()};

            var model = _sut.ConvertToModel(target);

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof (FakeModel));

            Assert.AreEqual(target.Name, ((FakeModel) model).Name);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidCastException))]
        public void ConvertFromModel_NotInstanceOfModel_InvalidCastException()
        {
            var model = new FakeNotImplClass();

            var target = _sut.ConvertFromModel(model);
        }

        [TestMethod]
        public void ConvertFromModel_ReturnsTargetInstance()
        {
            var model = new FakeModel {Name = _fixture.Create<string>()};

            var target = _sut.ConvertFromModel(model);

            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof (IFakeInterface));
            Assert.AreEqual(model.Name, ((IFakeInterface) target).Name);
        }
    }
}