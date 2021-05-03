using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class LoginPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserver;
        private Mock<IAuthService> mockAuthService;

        public LoginPageViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockNavigationService = mockRepository.Create<INavigationService>();
            mockDialogService = mockRepository.Create<IDialogService>();
            mockDocumentObserver = mockRepository.Create<IDocumentObserver<User>>();
            mockAuthService = mockRepository.Create<IAuthService>();
        }

        private LoginPageViewModel CreateViewModel()
        {
            return new LoginPageViewModel(
                mockNavigationService.Object,
                mockDialogService.Object,
                mockDocumentObserver.Object,
                mockAuthService.Object);
        }

        [Fact]
        public void LoginCommandCanExecute_EmailIsNull_CannotExecute()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act
            viewModel.Email = null;

            // Assert
            Assert.False(viewModel.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void LoginCommandCanExecute_PasswordIsNull_CannotExecute()
        {
            // Arrange
            var viewModel = CreateViewModel();

            // Act
            viewModel.Password = null;

            // Assert
            Assert.False(viewModel.LoginCommand.CanExecute(null));
        }
    }
}
