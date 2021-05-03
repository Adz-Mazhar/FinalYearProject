using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class JoinGroupPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserver;
        private Mock<IGroupDBService> mockGroupDBService;

        public JoinGroupPageViewModelTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockNavigationService = this.mockRepository.Create<INavigationService>();
            this.mockDialogService = this.mockRepository.Create<IDialogService>();
            this.mockDocumentObserver = this.mockRepository.Create<IDocumentObserver<User>>();
            this.mockGroupDBService = this.mockRepository.Create<IGroupDBService>();

            var groupIds = new List<string> { "Id1", "Id2", "Id3", "Id4" };

            mockDocumentObserver.SetupGet(o => o.Document).Returns(new User
            {
                JoinedGroups = new() { groupIds[0] },
                OwnedGroups = new()
            });
            mockGroupDBService.Setup(s => s.GetAllGroups()).Returns(Task.FromResult((IList<Group>)new List<Group>
            {
                new Group { Id = groupIds[0], Category = GroupCategory.Anxiety },
                new Group { Id = groupIds[1], Category = GroupCategory.Anxiety },
                new Group { Id = groupIds[2], Category = GroupCategory.Family },
                new Group { Id = groupIds[3], Category = GroupCategory.Relationships },
            }));
        }

        private JoinGroupPageViewModel CreateViewModel()
        {
            var viewModel = new JoinGroupPageViewModel(
                this.mockNavigationService.Object,
                this.mockDialogService.Object,
                this.mockDocumentObserver.Object,
                this.mockGroupDBService.Object);

            viewModel.OnNavigatedTo(null);

            return viewModel;
        }

        [Fact]
        public void SelectCategoryCommand_SelectFamilyCategory_GroupsSet()
        {
            // Arrange
            var viewModel = this.CreateViewModel();

            viewModel.SelectCategoryCommand.Execute((int?)GroupCategory.Anxiety);

            // Assert
            Assert.True(viewModel.Groups.Count is 1);
            this.mockRepository.VerifyAll();
        }
    }
}
