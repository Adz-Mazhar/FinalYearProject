using Plugin.CloudFirestore.Attributes;
using System.Collections.Generic;

namespace FinalYearProject.Models
{
    public record LikeablePost : Sendable
    {
        public LikeablePost()
        {
        }

        public LikeablePost(User creator, string text) : base(creator?.Id, text)
        {
            SenderInfo = new UserDisplayInfo(creator);
            LikedBy = new List<string>();
        }

        [Id]
        public string Id { get; init; }

        public UserDisplayInfo SenderInfo { get; init; }

        public List<string> LikedBy { get; init; }

        [Ignored]
        public bool IsLikedByUser { get; set; }
    }
}
