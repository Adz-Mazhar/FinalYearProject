using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.User;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class ChangeUsernamePageViewModelTests
    {
        Mock<INavigationService> navServiceMock;
        Mock<IDialogService> dialogServiceMock;
        Mock<IDocumentObserver<User>> userObserverMock;
        Mock<IUserDBService> userDBServiceMock;

        public ChangeUsernamePageViewModelTests()
        {
            navServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            userObserverMock = new Mock<IDocumentObserver<User>>();
            userDBServiceMock = new Mock<IUserDBService>();
        }

        [Fact]
        public void CanExecute_NewUsernameIsNull_ReturnsFalse()
        {
            var viewModel = new ChangeUsernamePageViewModel(navServiceMock.Object, dialogServiceMock.Object, userObserverMock.Object, userDBServiceMock.Object)
            {
                NewUsername = null
            };

            Assert.False(viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void CanExecute_NewUsernameIsValid_ReturnsTrue()
        {
            var viewModel = new ChangeUsernamePageViewModel(navServiceMock.Object, dialogServiceMock.Object, userObserverMock.Object, userDBServiceMock.Object)
            {
                NewUsername = "Orc",
            };

            Assert.True(viewModel.SaveCommand.CanExecute(null));
        }
    }
}
