using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Post;
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
    public class GroupPostsPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserverUser;
        private Mock<IDocumentObserver<Group>> mockDocumentObserverGroup;
        private Mock<IPostDBService> mockPostDBService;

        public GroupPostsPageViewModelTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockNavigationService = this.mockRepository.Create<INavigationService>();
            this.mockDialogService = this.mockRepository.Create<IDialogService>();
            this.mockDocumentObserverUser = this.mockRepository.Create<IDocumentObserver<User>>();
            this.mockDocumentObserverGroup = this.mockRepository.Create<IDocumentObserver<Group>>();
            this.mockPostDBService = this.mockRepository.Create<IPostDBService>();
        }

        private GroupPostsPageViewModel CreateViewModel()
        {
            return new GroupPostsPageViewModel(
                this.mockNavigationService.Object,
                this.mockDialogService.Object,
                this.mockDocumentObserverUser.Object,
                this.mockDocumentObserverGroup.Object,
                this.mockPostDBService.Object);
        }

        [Fact]
        public void Initialize_PostsLoadedInDescendingOrder()
        {
            // Arrange
            var firstPost = new Post 
            { 
                SentAt = new Plugin.CloudFirestore.Timestamp(DateTime.Now - TimeSpan.FromDays(5)),
                LikedBy = new(),
            };
            var secondPost = new Post
            {
                SentAt = new Plugin.CloudFirestore.Timestamp(DateTime.Now - TimeSpan.FromDays(3)),
                LikedBy = new(),
            };
            var thirdPost = new Post
            {
                SentAt = new Plugin.CloudFirestore.Timestamp(DateTime.Now - TimeSpan.FromDays(1)),
                LikedBy = new(),
            };

            mockPostDBService.Setup(s => s.GetAllPostsAsync(It.IsAny<string>(), nameof(Post.SentAt))).Returns(Task.FromResult((IList<Post>)new List<Post>
            {
                firstPost, secondPost, thirdPost
            }));

            mockDocumentObserverUser.SetupGet(o => o.Document).Returns(new User { Id = "" });
            mockDocumentObserverGroup.SetupGet(o => o.Document).Returns(new Group { Id = "" });

            var viewModel = this.CreateViewModel();

            // Act
            viewModel.Initialize(null);

            // Assert
            Assert.True(viewModel.Posts[0] == thirdPost && viewModel.Posts[1] == secondPost && viewModel.Posts[2] == firstPost);
        }

        [Fact]
        public void UpdateLikeCommand_LikesPost()
        {
            mockPostDBService
                .Setup(s => s.UpdatePostLikesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LikeOptions>()))
                .Returns(Task.CompletedTask);

            mockDocumentObserverUser.SetupGet(o => o.Document).Returns(new User { Id = "User" });
            mockDocumentObserverGroup.SetupGet(o => o.Document).Returns(new Group { Id = "Group" });

            var viewmodel = CreateViewModel();

            viewmodel.Posts.Add(new Post { Id = "Post", LikedBy = new(), IsLikedByUser = false });

            viewmodel.UpdateLikeCommand.Execute(viewmodel.Posts[0]);

            Assert.True(viewmodel.Posts[0].IsLikedByUser is true);
            Assert.Contains("User", viewmodel.Posts[0].LikedBy);
        }
    }
}
