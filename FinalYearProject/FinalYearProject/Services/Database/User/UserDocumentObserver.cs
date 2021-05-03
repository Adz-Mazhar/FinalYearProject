using FinalYearProject.Events;
using FinalYearProject.Extensions;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace FinalYearProject.Services.Database.User
{
    public class UserDocumentObserver : BindableBase, IDocumentObserver<Models.User>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IUserDBService userDBService;
        private IDisposable databaseListener;

        public UserDocumentObserver(IEventAggregator eventAggregator,
                                    IUserDBService userDBService)
        {
            this.eventAggregator = eventAggregator;
            this.userDBService = userDBService;
        }

        public Models.User Document { get; private set; }

        public bool IsObserving { get; private set; }

        public void BeginObserving(string userId)
        {
            userId.ThrowIfNull(nameof(userId));

            databaseListener = userDBService.ObserveUser(userId, RefreshUser);
            IsObserving = true;
        }

        public void StopObserving()
        {
            databaseListener?.Dispose();
            databaseListener = null;

            Document = null;

            IsObserving = false;
        }

        private void RefreshUser(Models.User user)
        {
            Document = user;

            eventAggregator.GetEvent<UserChangedEvent>().Publish();
        }
    }
}
