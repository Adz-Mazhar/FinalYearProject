using FinalYearProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Post
{
    public interface IPostDBService
    {
        Task AddPostAsync(Models.Post post, string groupId, string newId = null);
        Task AddReplyAsync(Reply reply, string groupId, string postId, string newId = null);
        Task<IList<Models.Post>> GetAllPostsAsync(string groupId, string orderByField = null);
        Task<IList<Reply>> GetAllRepliesAsync(string groupId, string postId, string orderByField = null);
        Task<Models.Post> GetPostAsync(string groupId, string postId);
        Task<IList<Models.Post>> GetPostsAsync(string groupId, IEnumerable<string> postIds);
        Task<IList<Models.Post>> GetPostsAsync(IEnumerable<string> postIds);
        Task UpdatePostLikesAsync(string groupId, string postId, string likerId, LikeOptions option);
        Task UpdateReplyLikesAsync(string groupId, string postId, string replyId, string likerId, LikeOptions option);
    }
}