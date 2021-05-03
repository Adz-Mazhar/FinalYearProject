using System;
using System.Collections.Generic;

namespace FinalYearProject.Services.Database.Message
{
    public interface IMessageCollectionObserver : ISubCollectionObserver<Models.Message>
    {
        void BeginObserving(string groupId, Action<Models.Message> onMessageAdded);
        IList<Models.Message> GetMessages(int count, int skipCount, bool fromEnd = false);
    }
}
