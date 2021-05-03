using FinalYearProject.Extensions;
using System;
using Xunit;

namespace FinalYearProject.Tests.Helpers
{
    public class NullCheckExtensionsTests
    {
        [Fact]
        public void ThrowIfNull_NullString_ThrowsArgumentNullException()
        {
            // Arrange
            string str = null;

            // Act
            void action() => NullCheckExtensions.ThrowIfNull(str, nameof(str));

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ThrowIfNullOrEmpty_EmptyString_ThrowsArgumentNullException()
        {
            // Arrange
            string str = "";

            // Act
            void action() => NullCheckExtensions.ThrowIfNullOrEmpty(str, nameof(str));

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
