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
    public class ChangeGroupNamePageViewModelTests
    {
        Mock<INavigationService> navServiceMock;
        Mock<IDialogService> dialogServiceMock;
        Mock<IDocumentObserver<User>> userObserverMock;
        Mock<IDocumentObserver<Group>> groupObserverMock;
        Mock<IGroupDBService> groupDBServiceMock;

        public ChangeGroupNamePageViewModelTests()
        {
            navServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            userObserverMock = new Mock<IDocumentObserver<User>>();
            groupObserverMock = new Mock<IDocumentObserver<Group>>();
            groupDBServiceMock = new Mock<IGroupDBService>();
        }

        [Fact]
        public void CanExecute_NewGroupNameIsNull_ReturnsFalse()
        {
            var viewModel = new ChangeGroupNamePageViewModel(navServiceMock.Object, dialogServiceMock.Object, userObserverMock.Object, groupObserverMock.Object, groupDBServiceMock.Object)
            {
                NewGroupName = null
            };

            Assert.False(viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void CanExecute_NewGroupNameIsValid_ReturnsTrue()
        {
            var viewModel = new ChangeGroupNamePageViewModel(navServiceMock.Object, dialogServiceMock.Object, userObserverMock.Object, groupObserverMock.Object, groupDBServiceMock.Object)
            {
                NewGroupName = "New"
            };

            Assert.True(viewModel.SaveCommand.CanExecute(null));
        }
    }
}
