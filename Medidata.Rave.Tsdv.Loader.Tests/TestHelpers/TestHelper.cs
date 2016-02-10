using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Medidata.Cloud.ExcelLoader.Validations;
using Medidata.Interfaces.Localization;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;
using Rhino.Mocks;

namespace Medidata.Rave.Tsdv.Loader.Tests.TestHelpers
{
    public static class TestHelper
    {
        private static readonly IFixture Fixture = new Fixture().Customize(new AutoRhinoMockCustomization());

        public static IFixture GetFixture(this object target)
        {
            return Fixture;
        }

        public static ILocalization CreateStubLocalization()
        {
            var localization = Fixture.Create<ILocalization>();
            localization.Stub(x => x.GetLocalString(null)).IgnoreArguments().Return(null).WhenCalled(a =>
            {
                var id = a.Arguments.FirstOrDefault();
                a.ReturnValue = "[" + id + "]";
            });
            return localization;
        } 
    }
}