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
    public class ChangePasswordPageViewModelTests
    {
        Mock<INavigationService> navServiceMock;
        Mock<IDialogService> dialogServiceMock;
        Mock<IDocumentObserver<User>> userObserverMock;
        Mock<IAuthService> authServiceMock;

        public ChangePasswordPageViewModelTests()
        {
            navServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            userObserverMock = new Mock<IDocumentObserver<User>>();
            authServiceMock = new Mock<IAuthService>();
        }

        [Fact]
        public void CanExecute_NewPasswordIsNull_CannotExecute()
        {
            var viewModel = new ChangePasswordPageViewModel(navServiceMock.Object, dialogServiceMock.Object, userObserverMock.Object, authServiceMock.Object)
            {
                NewPassword = null
            };

            Assert.False(viewModel.SavePasswordCommand.CanExecute(null));
        }

        [Fact]
        public void CanExecute_NewPasswordIsValid_CannotExecute()
        {
            var viewModel = new ChangePasswordPageViewModel(navServiceMock.Object, dialogServiceMock.Object, userObserverMock.Object, authServiceMock.Object)
            {
                NewPassword = "Orc",
            };

            Assert.True(viewModel.SavePasswordCommand.CanExecute(null));
        }
    }
}
