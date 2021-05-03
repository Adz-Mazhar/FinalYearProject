using FinalYearProject.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FinalYearProject.Tests.Helpers
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void Split_ListCountIs6AndBatchSizeis2_ListIsSplitInCorrectNumberOfBatches()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5, 6 };
            int batchSize = 2;

            // Act
            var splitLists = IEnumerableExtensions.Split(list, batchSize);

            // Assert
            Assert.True(splitLists.Count() is 3 && splitLists.All(l => l.Count() is 2));
        }

        [Fact]
        public void Split_ListCountIs8AndBatchSizeIs3_ListIsSplitInCorrectNumberOfBatches()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            int batchSize = 3;

            // Act
            var splitLists = IEnumerableExtensions.Split(list, batchSize);

            // Assert
            Assert.True(splitLists.Count() is 3
                && splitLists.ElementAt(0).Count() is 3
                && splitLists.ElementAt(1).Count() is 3
                && splitLists.ElementAt(2).Count() is 2);
        }

        [Fact]
        public void TotalCount_TotalNumberOItemsIs6_Returns6()
        {
            // Arrange
            var listOfLists = new List<List<string>>
            {
                new List<string> { "", "" },
                new List<string> { "", "", ""},
                new List<string> { "" },
            };

            // Act
            var result = IEnumerableExtensions.TotalCount(listOfLists);

            // Assert
            Assert.True(result is 6);
        }
    }
}
