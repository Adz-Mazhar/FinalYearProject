using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Activity;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class NewGroupActivityPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IEventAggregator> mockEventAggregator;
        private Mock<IDocumentObserver<User>> mockDocumentObserver;
        private Mock<IActivityDBService> mockActivityDBService;

        public NewGroupActivityPageViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockNavigationService = mockRepository.Create<INavigationService>();
            mockDialogService = mockRepository.Create<IDialogService>();
            mockEventAggregator = mockRepository.Create<IEventAggregator>();
            mockDocumentObserver = mockRepository.Create<IDocumentObserver<User>>();
            mockActivityDBService = mockRepository.Create<IActivityDBService>();
        }

        private NewGroupActivityPageViewModel CreateViewModel()
        {
            return new NewGroupActivityPageViewModel(
                mockNavigationService.Object,
                mockDialogService.Object,
                mockDocumentObserver.Object,
                mockEventAggregator.Object,
                mockActivityDBService.Object);
        }

        [Fact]
        public void CreateActivityCommandCanExecute_ActivityTitleIsNull_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.ActivityTitle = null;

            // Assert
            Assert.False(viewModel.CreateActivityCommand.CanExecute(null));
        }
    }
}
