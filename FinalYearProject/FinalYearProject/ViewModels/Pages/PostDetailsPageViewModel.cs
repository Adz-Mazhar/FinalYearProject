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
    public class PostDetailsPageViewModel : GroupRequiringViewModel
    {
        private readonly IPostDBService postDBService;
        private string postId;

        public PostDetailsPageViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IDocumentObserver<User> userObserver,
                                        IDocumentObserver<Group> groupObserver,
                                        IPostDBService postDBService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            this.postDBService = postDBService;

            Replies = new ObservableCollection<Reply>();

            AddReplyCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("NewReplyPage", new NavigationParameters
                {
                    { "groupId", GroupObserver.Document.Id },
                    { "postId", Post.Id },
                });
            });

            UpdatePostLikeCommand = new DelegateCommand(async () =>
            {
                await UpdatePostLikeCountAsync();
                RaisePropertyChanged(nameof(Post));
            });

            UpdateReplyLikeCommand = new DelegateCommand<Reply>(async reply =>
            {
                await UpdateReplyLikeCountAsync(reply);
                ReplaceReply(reply);
            });
        }

        public Post Post { get; private set; }

        public ObservableCollection<Reply> Replies { get; private set; }

        public ICommand AddReplyCommand { get; private set; }

        public ICommand UpdatePostLikeCommand { get; private set; }

        public ICommand UpdateReplyLikeCommand { get; private set; }

        public ICommand ReportCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            postId = parameters.GetValue<string>("postId");
            postId.ThrowIfNull(nameof(parameters), "Parameters must include post id.");

            await GetPostAsync();
            await GetRepliesAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<bool>("shouldRefresh"))
            {
                await RefreshAsync();
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add("shouldRefresh", true);
        }

        private async Task RefreshAsync()
        {
            await GetPostAsync();

            Replies.Clear();
            await GetRepliesAsync();
        }

        private async Task GetPostAsync()
        {
            var post = await postDBService.GetPostAsync(GroupObserver.Document.Id, postId);
            SetPostInfo(post);

            Post = post;
        }

        private async Task GetRepliesAsync()
        {
            var replies = await postDBService.GetAllRepliesAsync(GroupObserver.Document.Id, postId, nameof(Sendable.SentAt));

            foreach (var reply in replies)
            {
                SetPostInfo(reply);
                Replies.Add(reply);
            }
        }

        private void SetPostInfo(LikeablePost post)
        {
            post.IsLikedByUser = post.LikedBy.Contains(UserObserver.Document.Id);
        }

        private async Task UpdatePostLikeCountAsync()
        {
            LikeOptions likeCountOption = GetLikeCountUpdateOption(Post);

            UpdateLikeInfo(Post);
            
            await postDBService.UpdatePostLikesAsync(GroupObserver.Document.Id, postId, UserObserver.Document.Id, likeCountOption);
        }

        private async Task UpdateReplyLikeCountAsync(Reply reply)
        {
            LikeOptions likeCountOption = GetLikeCountUpdateOption(reply);

            UpdateLikeInfo(reply);
            
            await postDBService.UpdateReplyLikesAsync(GroupObserver.Document.Id, postId, reply.Id, UserObserver.Document.Id, likeCountOption);
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

        private void ReplaceReply(Reply newReply)
        {
            int replyIndex = Replies.IndexOf(Replies.Where(p => p.Id == newReply.Id).FirstOrDefault());
            Replies[replyIndex] = newReply;
        }

        private LikeOptions GetLikeCountUpdateOption(LikeablePost post)
        {
            return post.IsLikedByUser ? LikeOptions.Unlike : LikeOptions.Like;
        }
    }
}
