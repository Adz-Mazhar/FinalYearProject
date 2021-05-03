using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Post;
using FinalYearProject.ViewModels.Base;
using FinalYearProject.ViewModels.Helpers;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class GroupPostsPageViewModel : GroupRequiringViewModel
    {
        private readonly IPostDBService postDBService;

        public GroupPostsPageViewModel(INavigationService navigationService,
                                       IDialogService dialogService,
                                       IDocumentObserver<User> userObserver,
                                       IDocumentObserver<Group> groupObserver,
                                       IPostDBService postDBService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            this.postDBService = postDBService;

            Title = "Posts";

            Posts = new ObservableCollection<Post>();

            AddPostCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("NewPostPage", new NavigationParameters
                {
                    { "groupId", GroupObserver.Document.Id }
                });
            });

            UpdateLikeCommand = new DelegateCommand<Post>(async post =>
            {
                LikeOptions option = GetLikeOption(post);

                UpdateLikeInfo(post);
                ReplacePost(post);

                await postDBService.UpdatePostLikesAsync(GroupObserver.Document.Id, post.Id, UserObserver.Document.Id, option);
            });

            ReplyCommand = new DelegateCommand<Post>(async post =>
            {
                await NavigationService.NavigateAsync("PostDetailsPage", new NavigationParameters
                {
                    { "postId",  post.Id},
                });
            });
        }

        public ObservableCollection<Post> Posts { get; private set; }

        public ICommand AddPostCommand { get; private set; }

        public ICommand UpdateLikeCommand { get; private set; }

        public ICommand ReplyCommand { get; private set; }

        public ICommand ReportCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            await GetPostsAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<bool>("shouldRefresh"))
            {
                await RefreshAsync();
            }
        }

        private async Task RefreshAsync()
        {
            Posts.Clear();
            await GetPostsAsync();
        }

        private async Task GetPostsAsync()
        {
            var posts = await postDBService.GetAllPostsAsync(GroupObserver.Document.Id, nameof(Post.SentAt));
            var reversedPosts = posts.Reverse();

            foreach (var post in reversedPosts)
            {
                post.IsLikedByUser = post.LikedBy.Contains(UserObserver.Document.Id);
                Posts.Add(post);
            }
        }

        private void UpdateLikeInfo(LikeablePost post)
        {
            if (post.IsLikedByUser)
            {
                post.LikedBy.Remove(UserObserver.Document.Id);
            }
            else
            {
                post.LikedBy.Add(UserObserver.Document.Id);
            }

            post.IsLikedByUser = !post.IsLikedByUser;
        }

        private void ReplacePost(Post newPost)
        {
            int postIndex = Posts.IndexOf(Posts.Where(p => p.Id == newPost.Id).FirstOrDefault());
            Posts[postIndex] = newPost;
        }

        private LikeOptions GetLikeOption(LikeablePost post)
        {
            return post.IsLikedByUser ? LikeOptions.Unlike : LikeOptions.Like;
        }
    }
}
