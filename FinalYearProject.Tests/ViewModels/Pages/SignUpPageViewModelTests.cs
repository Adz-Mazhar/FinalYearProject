using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.User;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class SignUpPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserver;
        private Mock<IAuthService> mockAuthService;
        private Mock<IUserDBService> mockUserDBService;

        public SignUpPageViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockNavigationService = mockRepository.Create<INavigationService>();
            mockDialogService = mockRepository.Create<IDialogService>();
            mockDocumentObserver = mockRepository.Create<IDocumentObserver<User>>();
            mockAuthService = mockRepository.Create<IAuthService>();
            mockUserDBService = mockRepository.Create<IUserDBService>();
        }

        private SignUpPageViewModel CreateViewModel()
        {
            return new SignUpPageViewModel(
                mockNavigationService.Object,
                mockDialogService.Object,
                mockDocumentObserver.Object,
                mockAuthService.Object,
                mockUserDBService.Object);
        }

        [Fact]
        public void SignUpCommandCanExecute_UsernameIsNull_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.Username = null;
            // Act

            // Assert
            Assert.False(viewModel.SignUpCommand.CanExecute(null));
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SignUpCommandCanExecute_EmailIsNull_CannotExecute()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.Email = null;
            // Act

            // Assert
            Assert.False(viewModel.SignUpCommand.CanExecute(null));
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SignUpCommandCanExecute_PasswordIsNull_CannotExecute()
        {
            // Arrange
            var viewModel = CreateViewModel();
            viewModel.Password = null;
            // Act

            // Assert
            Assert.False(viewModel.SignUpCommand.CanExecute(null));
            mockRepository.VerifyAll();
        }
    }
}
