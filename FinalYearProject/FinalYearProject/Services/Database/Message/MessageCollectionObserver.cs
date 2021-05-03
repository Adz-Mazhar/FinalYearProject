using FinalYearProject.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.Services.Database.Message
{
    public class MessageCollectionObserver : IMessageCollectionObserver
    {
        private readonly IMessageDBService messageDBService;
        private IDisposable databaseListener;

        public MessageCollectionObserver(IMessageDBService messageDBService)
        {
            this.messageDBService = messageDBService;

            Collection = new List<Models.Message>();
        }

        public List<Models.Message> Collection { get; private set; }

        public bool IsObserving { get; private set; }

        public void BeginObserving(string groupId)
        {
            BeginObserving(groupId, null);
        }

        public void BeginObserving(string groupId, Action<Models.Message> onMessageAdded)
        {
            groupId.ThrowIfNull(nameof(groupId));

            databaseListener = messageDBService.ObserveMessages(groupId, message =>
            {
                AddToCollection(message);
                onMessageAdded?.Invoke(message);
            });
            IsObserving = true;
        }

        public void StopObserving()
        {
            databaseListener?.Dispose();
            databaseListener = null;

            Collection.Clear();

            IsObserving = false;
        }

        public IList<Models.Message> GetMessages(int count, int skipCount, bool fromEnd = false)
        {
            var collectionToGetFrom = fromEnd
                ? Collection.Reverse<Models.Message>()
                : Collection;

            return collectionToGetFrom.Skip(skipCount).Take(count).ToList();
        }

        private void AddToCollection(Models.Message message)
        {
            Collection.Add(message);
        }
    }
}
