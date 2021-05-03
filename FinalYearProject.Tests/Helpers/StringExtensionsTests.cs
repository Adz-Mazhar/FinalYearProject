using FinalYearProject.Extensions;
using Xunit;

namespace FinalYearProject.Tests.Helpers
{
    public class StringExtensionsTests
    {
        [Fact]
        public void AddSpaceBeforeCapitalLetters_StringWithoutSpaces_SameStringWithSpaces()
        {
            // Arrange
            string text = "ANormalString";

            // Act
            string textWithSpaces = StringExtensions.AddSpaceBeforeCapitalLetters(text);

            // Assert
            Assert.Equal("A Normal String", textWithSpaces);
        }

        [Fact]
        public void RemoveSpaces_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "A Normal String";

            // Act
            string textWithoutSpaces = StringExtensions.RemoveSpaces(text);

            // Assert
            Assert.Equal("ANormalString", textWithoutSpaces);
        }
    }
}
