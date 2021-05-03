using FinalYearProject.Services.Database.Transactions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace FinalYearProject.Tests.Services.Database
{
    public class TransactionTaskTests
    {
        private MockRepository mockRepository;

        public TransactionTaskTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private TransactionTask<T> CreateTransactionTask<T>()
        {
            return new TransactionTask<T>();
        }

        [Fact]
        public void Invoke_ListParameter_NewItemIsAddedToList()
        {
            // Arrange
            var transactionTask = CreateTransactionTask<List<int>>();
            transactionTask.Action = list => list.Add(3);

            var list = new List<int> { 0, 1, 2 };
            object[] parameters = new object[] { list };

            // Act
            var result = transactionTask.Invoke(parameters);

            // Assert
            Assert.True(list.Count is 4);
        }
    }
}
