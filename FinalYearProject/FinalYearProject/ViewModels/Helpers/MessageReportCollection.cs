using FinalYearProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.ViewModels.Helpers
{
    public class MessageReportCollection : List<MessageReport>
    {
        public string SenderId { get; }
        public string SenderUsername { get; }

        public MessageReportCollection(string senderId, string senderUsername) : this(senderId, senderUsername, Enumerable.Empty<MessageReport>())
        {
        }

        public MessageReportCollection(string senderId, string senderUsername, IEnumerable<MessageReport> messages) : base(messages)
        {
            SenderId = senderId;
            SenderUsername = senderUsername;
        }
    }
}
