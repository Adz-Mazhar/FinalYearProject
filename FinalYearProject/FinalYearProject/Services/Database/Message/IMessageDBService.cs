using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Message
{
    public interface IMessageDBService
    {
        Task DeleteAllMessagesFromUser(string groupId, string userId);
        Task<int> GetExistingMessageCount(string groupId);
        Task<IList<Models.Message>> GetMessagesAsync(string groupId, IEnumerable<string> messageIds);
        IDisposable ObserveMessages(string groupId, Action<Models.Message> onMessageAdded);
        Task SendMessageAsync(Models.Message message, string groupId, string newId = null);
    }
}