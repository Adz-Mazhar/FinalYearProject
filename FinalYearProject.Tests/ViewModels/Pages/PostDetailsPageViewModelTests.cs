using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Post;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class PostDetailsPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserverUser;
        private Mock<IDocumentObserver<Group>> mockDocumentObserverGroup;
        private Mock<IPostDBService> mockPostDBService;

        public PostDetailsPageViewModelTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockNavigationService = this.mockRepository.Create<INavigationService>();
            this.mockDialogService = this.mockRepository.Create<IDialogService>();
            this.mockDocumentObserverUser = this.mockRepository.Create<IDocumentObserver<User>>();
            this.mockDocumentObserverGroup = this.mockRepository.Create<IDocumentObserver<Group>>();
            this.mockPostDBService = this.mockRepository.Create<IPostDBService>();
        }

        private PostDetailsPageViewModel CreateViewModel()
        {
            return new PostDetailsPageViewModel(
                this.mockNavigationService.Object,
                this.mockDialogService.Object,
                this.mockDocumentObserverUser.Object,
                this.mockDocumentObserverGroup.Object,
                this.mockPostDBService.Object);
        }

        [Fact]
        public void UpdateReplyLikeCommand_LikesReply()
        {
            mockPostDBService
                .Setup(s => s.UpdateReplyLikesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LikeOptions>()))
                .Returns(Task.CompletedTask);

            mockDocumentObserverUser.SetupGet(o => o.Document).Returns(new User { Id = "User" });
            mockDocumentObserverGroup.SetupGet(o => o.Document).Returns(new Group { Id = "Group" });

            var viewmodel = CreateViewModel();

            viewmodel.Replies.Add(new Reply { Id = "Post", LikedBy = new(), IsLikedByUser = false });

            viewmodel.UpdateReplyLikeCommand.Execute(viewmodel.Replies[0]);

            Assert.True(viewmodel.Replies[0].IsLikedByUser is true);
            Assert.Contains("User", viewmodel.Replies[0].LikedBy);
        }
    }
}
