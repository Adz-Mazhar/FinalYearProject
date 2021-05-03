using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class NewGroupPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserver;
        private Mock<IGroupDBService> mockGroupDBService;

        public NewGroupPageViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockNavigationService = mockRepository.Create<INavigationService>();
            mockDialogService = mockRepository.Create<IDialogService>();
            mockDocumentObserver = mockRepository.Create<IDocumentObserver<User>>();
            mockGroupDBService = mockRepository.Create<IGroupDBService>();
        }

        private NewGroupPageViewModel CreateViewModel()
        {
            return new NewGroupPageViewModel(
                mockNavigationService.Object,
                mockDialogService.Object,
                mockDocumentObserver.Object,
                mockGroupDBService.Object);
        }

        [Fact]
        public void CreateGroupCommandCanExecute_GroupNameIsNull_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.GroupName = null;
            // Act


            // Assert
            Assert.False(viewModel.CreateGroupCommand.CanExecute(null));
        }
    }
}
