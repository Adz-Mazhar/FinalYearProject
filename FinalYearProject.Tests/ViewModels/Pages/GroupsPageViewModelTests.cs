using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class GroupsPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserverUser;
        private Mock<IGroupDBService> mockGroupDBService;
        private Mock<IDocumentObserver<Group>> mockDocumentObserverGroup;

        public GroupsPageViewModelTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockNavigationService = this.mockRepository.Create<INavigationService>();
            this.mockDialogService = this.mockRepository.Create<IDialogService>();
            this.mockDocumentObserverUser = this.mockRepository.Create<IDocumentObserver<User>>();
            this.mockGroupDBService = this.mockRepository.Create<IGroupDBService>();
            this.mockDocumentObserverGroup = this.mockRepository.Create<IDocumentObserver<Group>>();
        }

        private GroupsPageViewModel CreateViewModel()
        {
            return new GroupsPageViewModel(
                this.mockNavigationService.Object,
                this.mockDialogService.Object,
                this.mockDocumentObserverUser.Object,
                this.mockGroupDBService.Object,
                this.mockDocumentObserverGroup.Object);
        }

        [Fact]
        public void Initialize_GroupsAddedToCollection()
        {
            // Arrange
            var viewModel = this.CreateViewModel();
            INavigationParameters parameters = null;

            List<string> joinedGroups = new() { "Id1" };
            List<string> ownedGroups = new() { "Id2" };

            mockDocumentObserverUser.SetupGet(o => o.Document).Returns(new User 
            { 
                JoinedGroups = joinedGroups,
                OwnedGroups = ownedGroups,
            });
            mockGroupDBService.Setup(s => s.GetGroupsAsync(It.IsAny<IEnumerable<string>>())).Returns(Task.FromResult((IList<Group>)new List<Group>
            {
                new Group(),
            }));

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.True(viewModel.Groups.Sum(x => x.Count) is 2);
        }
    }
}
