using FinalYearProject.Extensions;
using System.Collections.ObjectModel;
using Xunit;

namespace FinalYearProject.Tests.Helpers
{
    public class ObservableCollectionExtensionsTests
    {
        [Fact]
        public void RemoveAll_CollectionWithMulitpleSameValues_RemovesAll()
        {
            // Arrange
            var collection = new ObservableCollection<int> { 0, 1, 1, 1, 2, 1, 3, 4 };

            // Act
            var numRemoved = ObservableCollectionExtensions.RemoveAll(collection, i => i is 1);

            // Assert
            Assert.True(numRemoved is 4 && !collection.Contains(1));
        }
    }
}
