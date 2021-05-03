using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Activity;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class GroupActivitiesPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserverUser;
        private Mock<IDocumentObserver<Group>> mockDocumentObserverGroup;
        private Mock<IEventAggregator> mockEventAggregator;
        private Mock<IActivityDBService> mockActivityDBService;

        public GroupActivitiesPageViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockNavigationService = mockRepository.Create<INavigationService>();
            mockDialogService = mockRepository.Create<IDialogService>();
            mockDocumentObserverUser = mockRepository.Create<IDocumentObserver<User>>();
            mockDocumentObserverGroup = mockRepository.Create<IDocumentObserver<Group>>();
            mockEventAggregator = mockRepository.Create<IEventAggregator>();
            mockActivityDBService = mockRepository.Create<IActivityDBService>();
        }

        private GroupActivitiesPageViewModel CreateViewModel()
        {
            return new GroupActivitiesPageViewModel(
                mockNavigationService.Object,
                mockDialogService.Object,
                mockDocumentObserverUser.Object,
                mockDocumentObserverGroup.Object,
                mockEventAggregator.Object,
                mockActivityDBService.Object);
        }

        [Fact]
        public async Task Initialize_SetsActivitiesProperty()
        {
            var mockId = "Id";
            mockDocumentObserverGroup.SetupGet(o => o.Document).Returns(new Group { Id = mockId });
            mockActivityDBService.Setup(s => s.GetActivitiesAsync(mockId, null)).Returns(Task.FromResult((IList<Activity>)new List<Activity>
            {
                new Activity(),
                new Activity(),
            }));
            mockDocumentObserverUser.SetupGet(o => o.Document).Returns(new User { JoinedActivities = new() });
            // Arrange
            var viewModel = CreateViewModel();
            INavigationParameters parameters = null;

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.True(viewModel.Activities.Count is 2);
        }
    }
}
