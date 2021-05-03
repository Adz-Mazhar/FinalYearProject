using FinalYearProject.Extensions;
using FinalYearProject.Services.Database.Transactions;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Reactive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Message
{
    public class MessageDBService : BaseDBService, IMessageDBService
    {
        private static readonly string messageCountDocName = "Count";
        private static readonly string messagesCollectionName = "Messages";

        public MessageDBService()
        {
            RegisterTransactionTasks();
        }

        private enum CountDocTransactionType { Increment, Decrement }

        public async Task<int> GetExistingMessageCount(string groupId)
        {
            groupId.ThrowIfNull(nameof(groupId));

            var counter = await GetAsync<DatabaseCounter>(
                messageCountDocName,
                MessagesCollectionReference(groupId));

            return counter is null ? 0 : counter.Count;
        }

        public async Task<IList<Models.Message>> GetMessagesAsync(string groupId, IEnumerable<string> messageIds)
        {
            groupId.ThrowIfNull(nameof(groupId));
            messageIds.ThrowIfNull(nameof(messageIds));

            return await GetMultipleAsync<Models.Message>(messageIds, MessagesCollectionReference(groupId));
        }

        public async Task DeleteAllMessagesFromUser(string groupId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            userId.ThrowIfNull(nameof(userId));

            var query = MessagesCollectionReference(groupId).WhereEqualsTo(nameof(Models.Message.Sender), userId);
            var messagesFromUser = await query.GetAsync();

            foreach (var message in messagesFromUser.Documents)
            {
                await DeleteAsync<Models.Message>(message.Id, MessagesCollectionReference(groupId));
                await DecrementCountAsync(groupId);
            }
        }

        // Action is invoked initially for every document in the collection
        public IDisposable ObserveMessages(string groupId, Action<Models.Message> onMessageAdded)
        {
            groupId.ThrowIfNull(nameof(groupId));
            onMessageAdded.ThrowIfNull(nameof(onMessageAdded));

            var orderedMessages = MessagesCollectionReference(groupId).OrderBy(nameof(Models.Message.SentAt));

            var listener = orderedMessages
                .ObserveAdded()
                .Subscribe(documentChange =>
                {
                    var message = GetMessageFromSnapshot(documentChange.Document);
                    onMessageAdded.Invoke(message);
                });

            return listener;
        }

        public async Task SendMessageAsync(Models.Message message,
                                           string groupId,
                                           string newId = null)
        {
            message.ThrowIfNull(nameof(message));
            groupId.ThrowIfNull(nameof(groupId));

            await AddAsync(message, newId, MessagesCollectionReference(groupId));

            await IncrementCountAsync(groupId);
        }

        private void RegisterTransactionTasks()
        {
            RegisterMultipleTransactionTasks(new List<(string, ITransactionTask)>
            {
                (nameof(CountDocTransactionType.Increment), new TransactionTask<DatabaseCounter>
                {
                    Action = counter => counter.Count++
                }),

                (nameof(CountDocTransactionType.Decrement), new TransactionTask<DatabaseCounter>
                {
                    Action = counter => counter.Count++
                }),
            });
        }

        private IDocumentReference CountDocumentReference(string groupId)
        {
            return MessagesCollectionReference(groupId).Document(messageCountDocName);
        }

        private ICollectionReference MessagesCollectionReference(string groupId)
        {
            return GetBaseCollectionReference<Models.Group>()
                .Document(groupId)
                .Collection(messagesCollectionName);
        }

        private Models.Message GetMessageFromSnapshot(IDocumentSnapshot document)
        {
            return document.Metadata.HasPendingWrites
                ? document.ToObject<Models.Message>(ServerTimestampBehavior.Estimate)
                : document.ToObject<Models.Message>();
        }

        private async Task AddCountDocumentIfNotExistsAsync(string groupId)
        {
            var countDoc = await CountDocumentReference(groupId).GetAsync();

            if (!countDoc.Exists)
            {
                var counter = new DatabaseCounter { Count = 0 };
                await AddAsync(counter, messageCountDocName, MessagesCollectionReference(groupId));
            }
        }

        private async Task IncrementCountAsync(string groupId)
        {
            await AddCountDocumentIfNotExistsAsync(groupId);

            await RunTransactionAsync<DatabaseCounter>(CountDocumentReference(groupId),
                                                       nameof(CountDocTransactionType.Increment),
                                                       null);
        }

        private async Task DecrementCountAsync(string groupId)
        {
            await RunTransactionAsync<DatabaseCounter>(CountDocumentReference(groupId),
                                                       nameof(CountDocTransactionType.Decrement),
                                                       null);
        }
    }
}
