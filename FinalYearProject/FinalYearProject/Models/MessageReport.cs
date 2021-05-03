using Plugin.CloudFirestore.Attributes;

namespace FinalYearProject.Models
{
    public record MessageReport
    {
        public MessageReport()
        {
        }

        public MessageReport(string messageId, string messageSenderId, string description)
        {
            MessageId = messageId;
            MessageSenderId = messageSenderId;
            Description = description;
        }

        [Id]
        public string Id { get; init; }

        public string MessageSenderId { get; init; }

        public string MessageId { get; init; }

        public string Description { get; init; }

        [Ignored]
        public Message Message { get; set; }
    }
}
