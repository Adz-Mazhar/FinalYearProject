namespace FinalYearProject.Models
{
    public record Post : LikeablePost
    {
        public Post()
        {
        }

        public Post(User creator, string text) : base(creator, text)
        {
        }

        public int ReplyCount { get; set; }
    }
}
