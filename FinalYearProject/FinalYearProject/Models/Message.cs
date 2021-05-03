using Plugin.CloudFirestore.Attributes;

namespace FinalYearProject.Models
{
    public record Message : Sendable
    {
        public Message()
        {
        }

        public Message(User sender, string text) : base(sender?.Id, text)
        {
            SenderInfo = new UserDisplayInfo(sender);
        }

        [Id]
        public string Id { get; init; }

        public UserDisplayInfo SenderInfo { get; init; }

        [Ignored]
        public bool IsOwnMessage { get; set; }
    }
}
