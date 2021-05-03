using FinalYearProject.Events;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class GroupInfoPageViewModel : GroupRequiringViewModel
    {
        public GroupInfoPageViewModel(INavigationService navigationService,
                                      IDialogService dialogService,
                                      IDocumentObserver<User> userObserver,
                                      IDocumentObserver<Group> groupObserver,
                                      IEventAggregator eventAggregator)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            eventAggregator.GetEvent<GroupChangedEvent>().Subscribe(
                Refresh,
                ThreadOption.UIThread,
                false);

            Title = "Info";

            GoToReportsCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("GroupReportsPage");
            });
        }

        public string GroupName { get; private set; }

        public string GroupDescription { get; private set; }

        public bool IsOwner { get; private set; }

        public ICommand GoToReportsCommand { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            Refresh();
            IsOwner = GroupObserver.Document.Owner == UserObserver.Document.Id;
        }

        private void Refresh()
        {
            var group = GroupObserver.Document;

            GroupName = group.Name;
            GroupDescription = group.Description;
        }
    }
}
