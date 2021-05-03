using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace FinalYearProject.Models
{
    public abstract record Sendable
    {
        public Sendable()
        {
        }

        public Sendable(string sender, string text)
        {
            Sender = sender;
            Text = text;
        }

        public string Sender { get; init; }

        public string Text { get; init; }

        [ServerTimestamp(CanReplace = false)]
        public Timestamp SentAt { get; init; }
    }
}
