using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Base;
using FinalYearProject.ViewModels.Helpers;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskExtensions = FinalYearProject.Extensions.TaskExtensions;

namespace FinalYearProject.ViewModels.Pages
{
    public  class GroupsPageViewModel : BaseViewModel
    {
        private readonly IGroupDBService groupDBService;
        private readonly IDocumentObserver<Group> groupObserver;

        private readonly GroupCollection ownedGroups;
        private readonly GroupCollection joinedGroups;
        private readonly List<AsyncDialogOption> ownedGroupOptions;
        private readonly List<AsyncDialogOption> joinedGroupOptions;

        public GroupsPageViewModel(INavigationService navigationService,
                                   IDialogService dialogService,
                                   IDocumentObserver<User> userObserver,
                                   IGroupDBService groupDBService,
                                   IDocumentObserver<Group> groupObserver)
            : base(navigationService, dialogService, userObserver)
        {
            this.groupDBService = groupDBService;
            this.groupObserver = groupObserver;

            Title = "Groups";

            ownedGroups = new GroupCollection("My Groups");
            joinedGroups = new GroupCollection("Joined Groups");

            Groups = new List<GroupCollection> { ownedGroups, joinedGroups };

            ownedGroupOptions = new List<AsyncDialogOption>
            {
                new AsyncDialogOption
                {
                    Name = "Open",
                    ImageSource = "openicon.png",
                    Response = OpenGroupAsync,
                },
            };

            joinedGroupOptions = new List<AsyncDialogOption>
            {
                new AsyncDialogOption
                {
                    Name = "Open",
                    ImageSource = "openicon.png",
                    Response = OpenGroupAsync,
                },

                new AsyncDialogOption
                {
                    Name = "Leave",
                    ImageSource = "binicon.png",
                    Response = LeaveGroupAsync,
                },
            };

            JoinGroupCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("JoinGroupPage");
            });

            CreateGroupCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("NewGroupPage");
            });

            GroupTapCommand = new DelegateCommand<Group>(async group =>
            {
                SelectedGroup = group;

                if (SelectedGroup.BannedMembers.Contains(UserObserver.Document.Id))
                {
                    DialogExtensions.DisplayMessage(DialogService, "Banned!", "You have been banned from this group.");

                    await LeaveGroupAsync();
                    return;
                }

                IDialogParameters param = new DialogParameters();
                if (SelectedGroup.Owner == UserObserver.Document.Id)
                {
                    param.Add("options", ownedGroupOptions.Cast<DialogOptionBase>().ToList());
                }
                else
                {
                    param.Add("options", joinedGroupOptions.Cast<DialogOptionBase>().ToList());
                }

                DialogService.ShowDialog("OptionsDialog", param, OptionsDialogCallback);
            });

            RefreshCommand = new DelegateCommand(async () =>
            {
                ClearGroups();
                await InitializeGroupsAsync();

                IsRefreshing = false;
            });
        }

        public List<GroupCollection> Groups { get; private set; }

        public Group SelectedGroup { get; set; }

        public bool IsRefreshing { get; set; }

        public ICommand JoinGroupCommand { get; private set; }

        public ICommand CreateGroupCommand { get; private set; }

        public ICommand GroupTapCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            await InitializeGroupsAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await RefreshAsync();
        }

        private async Task InitializeGroupsAsync()
        {
            IsBusy = true;

            await GetAllGroups();

            IsBusy = false;
        }

        private async Task RefreshAsync()
        {
            IsBusy = true;

            var groupCount = Groups.Sum(x => x.Count);
            var userGroupCount = UserObserver.Document.OwnedGroups.Count + UserObserver.Document.JoinedGroups.Count;

            if (groupCount < userGroupCount)
            {
                await AddNewGroups();
            }
            else if (groupCount > userGroupCount)
            {
                RemoveLeftGroups();
            }

            IsBusy = false;
        }

        private async Task GetAllGroups()
        {
            var user = UserObserver.Document;

            await AddGroups(ownedGroups, user.OwnedGroups);
            await AddGroups(joinedGroups, user.JoinedGroups);
        }

        private async Task AddNewGroups()
        {
            var user = UserObserver.Document;

            if (user.OwnedGroups.Count != ownedGroups.Count)
            {
                var newIds = user.OwnedGroups.Skip(ownedGroups.Count).ToList();
                await AddGroups(ownedGroups, newIds);
            }
            else if (user.JoinedGroups.Count != joinedGroups.Count)
            {
                var newIds = user.JoinedGroups.Skip(joinedGroups.Count).ToList();
                await AddGroups(joinedGroups, newIds);
            }
        }

        private void RemoveLeftGroups()
        {
            var user = UserObserver.Document;

            if (user.OwnedGroups.Count != ownedGroups.Count)
            {
                var newIds = ownedGroups.Skip(user.OwnedGroups.Count).Select(g => g.Id).ToList();
                RemoveGroups(ownedGroups, newIds);
            }
            else if (user.JoinedGroups.Count != joinedGroups.Count)
            {
                var newIds = joinedGroups.Skip(user.JoinedGroups.Count).Select(g => g.Id).ToList();
                RemoveGroups(joinedGroups, newIds);
            }
        }

        private async Task AddGroups(ICollection<Group> collection, List<string> ids)
        {
            if (ids.Count is 0)
                return;

            var newGroups = (await groupDBService.GetGroupsAsync(ids)).ToList();

            newGroups.ForEach(g => collection.Add(g));
        }

        private void RemoveGroups(ICollection<Group> collection, List<string> ids)
        {
            if (ids.Count is 0)
                return;

            collection.RemoveAll(g => ids.Contains(g.Id));
        }

        private void ClearGroups()
        {
            Groups.ForEach(c => c.Clear());
        }

        private async Task OpenGroupAsync()
        {
            var navParams = new NavigationParameters
            {
                { "groupId", SelectedGroup.Id }
            };

            string navPath = SelectedGroup.Owner == UserObserver.Document.Id
                ? "GroupTabbedPage?createTab=GroupSettingsPage"
                : "GroupTabbedPage";

            groupObserver.BeginObserving(SelectedGroup.Id);
            await TaskExtensions.WaitUntil(() => groupObserver.Document is not null);

            await NavigationService.NavigateAsync(navPath, navParams);
        }

        private async Task LeaveGroupAsync()
        {
            await groupDBService.LeaveGroupAsync(SelectedGroup.Id, UserObserver.Document.Id);
            await RefreshAsync();
        }

        private async void OptionsDialogCallback(IDialogResult dialogResult)
        {
            var choice = dialogResult.Parameters.GetValue<DialogOptionBase>("choice") as AsyncDialogOption;

            if (choice is not null)
            {
                await choice.Response.Invoke();
            }
        }
    }
}
