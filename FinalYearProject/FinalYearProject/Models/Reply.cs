namespace FinalYearProject.Models
{
    public record Reply : LikeablePost
    {
        public Reply()
        {
        }

        public Reply(User creator, string text) : base(creator, text)
        {
        }
    }
}
