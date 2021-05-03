using FinalYearProject.Extensions;
using Xunit;

namespace FinalYearProject.Tests.Helpers
{
    public class GeneralExtensionsTests
    {
        [Fact]
        public void ConvertToDerived_BaseClassAToDerivedClass_DerivedClassContainsSamePropertyValuesAsBaseClass()
        {
            // Arrange
            var baseClassObj = new BaseClass { Number = 5, String = "A string" };

            // Act
            var derivedClassObj = GeneralExtensions.ConvertToDerived<BaseClass, DerivedClass>(baseClassObj);

            // Assert
            Assert.True(baseClassObj.Number == derivedClassObj.Number && baseClassObj.String == derivedClassObj.String);
        }

        private class BaseClass
        {
            public int Number { get; set; }

            public string String { get; set; }
        }

        private class DerivedClass : BaseClass
        {
        }
    }
}
