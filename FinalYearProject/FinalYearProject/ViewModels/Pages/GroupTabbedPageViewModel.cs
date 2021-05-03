using FinalYearProject.Dialogs;
using FinalYearProject.Events;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Message;
using FinalYearProject.ViewModels.Base;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace FinalYearProject.ViewModels.Pages
{
    class GroupTabbedPageViewModel : GroupRequiringViewModel
    {
        private readonly IMessageCollectionObserver messageCollectionObserver;

        private string groupId;

        public GroupTabbedPageViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IDocumentObserver<User> userObserver,
                                        IDocumentObserver<Group> groupObserver,
                                        IEventAggregator eventAggregator,
                                        IMessageCollectionObserver messageCollectionObserver)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            this.messageCollectionObserver = messageCollectionObserver;

            eventAggregator.GetEvent<UserBannedEvent>().Subscribe(
                OnUserBanned,
                ThreadOption.UIThread,
                false,
                bannedUserId => bannedUserId == UserObserver.Document.Id);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            groupId = parameters.GetValue<string>("groupId");
            groupId.ThrowIfNull(nameof(groupId));
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() is NavigationMode.Back)
            {
                if (GroupObserver.IsObserving)
                    GroupObserver.StopObserving();

                if (messageCollectionObserver.IsObserving)
                    messageCollectionObserver.StopObserving();
            }
        }

        private async void OnUserBanned(string bannedGroupId)
        {
            DialogExtensions.DisplayMessage(DialogService, "Banned!", "You have been banned from this group for inappropraite behaviour.");

            await NavigationService.GoBackToRootAsync();
        }
    }
}
