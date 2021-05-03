using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database.Transactions;
using Plugin.CloudFirestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Post
{
    public class PostDBService : BaseDBService, IPostDBService
    {
        private static readonly string postsCollectionName = "Posts";
        private static readonly string repliesCollectionName = "Replies";

        public PostDBService()
        {
            RegisterTransactionTask();
        }

        private enum PostTransactionType { IncrementReplyCount }
        private enum UserTransactionType { AddPost }

        public async Task AddPostAsync(Models.Post post, string groupId, string newId = null)
        {
            post.ThrowIfNull(nameof(post));
            post.Sender.ThrowIfNull(nameof(post.Sender));
            groupId.ThrowIfNull(nameof(groupId));

            IDocumentReference docRef = await AddAsync(post, newId, PostsCollectionReference(groupId));

            newId ??= docRef.Id;
            await RunUserTransactionAsync(post.Sender, UserTransactionType.AddPost, new object[] { newId });
        }

        public async Task AddReplyAsync(Reply reply, string groupId, string postId, string newId = null)
        {
            reply.ThrowIfNull(nameof(reply));
            reply.Sender.ThrowIfNull(nameof(reply.Sender));
            groupId.ThrowIfNull(nameof(groupId));
            postId.ThrowIfNull(nameof(postId));

            IDocumentReference docRef = await AddAsync(reply, newId, RepliesCollectionReference(groupId, postId));
            newId ??= docRef.Id;

            await RunReplyCountTransactionAsync(groupId, postId, PostTransactionType.IncrementReplyCount, null);
            await RunUserTransactionAsync(reply.Sender, UserTransactionType.AddPost, new object[] { newId });
        }

        public async Task<IList<Models.Post>> GetAllPostsAsync(string groupId, string orderByField = null)
        {
            groupId.ThrowIfNull(nameof(groupId));

            ICollectionReference postCollRef = PostsCollectionReference(groupId);
            IQuery query = orderByField is null ? postCollRef : postCollRef.OrderBy(orderByField);

            return await GetAllAsync<Models.Post>(query);
        }

        public async Task<IList<Models.Post>> GetPostsAsync(string groupId, IEnumerable<string> postIds)
        {
            groupId.ThrowIfNull(nameof(groupId));
            postIds.ThrowIfNull(nameof(postIds));

            return await GetMultipleAsync<Models.Post>(postIds, PostsCollectionReference(groupId));
        }

        public async Task<IList<Models.Post>> GetPostsAsync(IEnumerable<string> postIds)
        {
            postIds.ThrowIfNull(nameof(postIds));

            var collectionGroup = Firestore.CollectionGroup(postsCollectionName);
            return await GetMultipleAsync<Models.Post>(postIds, collectionGroup);
        }

        public async Task<Models.Post> GetPostAsync(string groupId, string postId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            postId.ThrowIfNull(nameof(postId));

            return await GetAsync<Models.Post>(postId, PostsCollectionReference(groupId));
        }

        public async Task<IList<Reply>> GetAllRepliesAsync(string groupId, string postId, string orderByField = null)
        {
            groupId.ThrowIfNull(nameof(groupId));
            postId.ThrowIfNull(nameof(postId));

            ICollectionReference replyCollRef = RepliesCollectionReference(groupId, postId);
            IQuery query = orderByField is null ? replyCollRef : replyCollRef.OrderBy(orderByField);

            return await GetAllAsync<Reply>(query);
        }

        public async Task UpdatePostLikesAsync(string groupId, string postId, string userId, LikeOptions option)
        {
            groupId.ThrowIfNull(nameof(groupId));
            postId.ThrowIfNull(nameof(postId));
            userId.ThrowIfNull(nameof(userId));

            IDocumentReference docRef = PostsCollectionReference(groupId).Document(postId);
            await RunLikeTransactionAsync(docRef, option, new object[] { userId });
        }

        public async Task UpdateReplyLikesAsync(string groupId, string postId, string replyId, string userId, LikeOptions option)
        {
            groupId.ThrowIfNull(nameof(groupId));
            postId.ThrowIfNull(nameof(postId));
            replyId.ThrowIfNull(nameof(replyId));

            IDocumentReference docRef = RepliesCollectionReference(groupId, postId).Document(replyId);
            await RunLikeTransactionAsync(docRef, option, new object[] { userId });
        }

        private async Task RunLikeTransactionAsync(IDocumentReference documentReference, LikeOptions transactionType, object[] parameters)
        {
            await RunTransactionAsync<LikeablePost>(
                documentReference,
                transactionType.ToString(),
                parameters);
        }

        private async Task RunReplyCountTransactionAsync(string groupId, string postId, PostTransactionType transactionType, object[] parameters)
        {
            await RunTransactionAsync<Models.Post>(
                PostsCollectionReference(groupId).Document(postId),
                transactionType.ToString(),
                parameters);
        }

        private async Task RunUserTransactionAsync(string userId, UserTransactionType transactionType, object[] parameters)
        {
            await RunTransactionAsync<Models.User>(
                GetBaseCollectionReference<Models.User>().Document(userId),
                transactionType.ToString(),
                parameters);
        }

        private void RegisterTransactionTask()
        {
            RegisterMultipleTransactionTasks(new List<(string, ITransactionTask)>
            {
                (nameof(LikeOptions.Like), new TransactionTask<LikeablePost, string>
                {
                    Action = (post, userId) => post.LikedBy.Add(userId)
                }),

                (nameof(LikeOptions.Unlike), new TransactionTask<LikeablePost, string>
                {
                    Action = (post, userId) => post.LikedBy.Remove(userId)
                }),

                (nameof(PostTransactionType.IncrementReplyCount), new TransactionTask<Models.Post>
                {
                    Action = post => post.ReplyCount++
                }),

                (nameof(UserTransactionType.AddPost), new TransactionTask<Models.User, string>
                {
                    Action = (user, postId) => user.Posts.Add(postId)
                }),
            });
        }

        private ICollectionReference PostsCollectionReference(string groupId)
        {
            return GetBaseCollectionReference<Models.Group>()
                .Document(groupId)
                .Collection(postsCollectionName);
        }

        private ICollectionReference RepliesCollectionReference(string groupId, string postId)
        {
            return PostsCollectionReference(groupId)
                .Document(postId)
                .Collection(repliesCollectionName);
        }
    }
}
