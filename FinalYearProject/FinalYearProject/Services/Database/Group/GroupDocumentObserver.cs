using FinalYearProject.Events;
using FinalYearProject.Extensions;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Linq;

namespace FinalYearProject.Services.Database.Group
{
    public class GroupDocumentObserver : BindableBase, IDocumentObserver<Models.Group>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IGroupDBService groupDBService;
        private IDisposable databaseListener;

        public GroupDocumentObserver(IEventAggregator eventAggregator, GroupDBService groupDBService)
        {
            this.eventAggregator = eventAggregator;
            this.groupDBService = groupDBService;
        }

        public Models.Group Document { get; private set; }

        public bool IsObserving { get; private set; }

        public void BeginObserving(string groupId)
        {
            groupId.ThrowIfNull(nameof(groupId));

            databaseListener = groupDBService.ObserveGroup(groupId, RefreshGroup);
            IsObserving = true;
        }

        public void StopObserving()
        {
            databaseListener?.Dispose();

            databaseListener = null;
            Document = null;
            IsObserving = false;
        }

        private void RefreshGroup(Models.Group group)
        {
            var oldGroup = Document;

            Document = group;

            eventAggregator.GetEvent<GroupChangedEvent>().Publish();

            if (Document.BannedMembers.Count > oldGroup?.BannedMembers.Count)
            {
                eventAggregator
                    .GetEvent<UserBannedEvent>()
                    .Publish(Document.BannedMembers.Except(oldGroup.BannedMembers).SingleOrDefault());
            }
        }
    }
}
