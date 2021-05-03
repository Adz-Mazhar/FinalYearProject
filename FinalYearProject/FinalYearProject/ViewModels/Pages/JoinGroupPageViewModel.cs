using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;

namespace FinalYearProject.ViewModels.Pages
{
    public class JoinGroupPageViewModel : BaseViewModel
    {
        private readonly IGroupDBService groupDBService;

        private List<Group> allGroups;

        public JoinGroupPageViewModel(INavigationService navigationService,
                                      IDialogService dialogService,
                                      IDocumentObserver<User> userObserver,
                                      IGroupDBService groupDBService)
            : base(navigationService, dialogService, userObserver)
        {
            this.groupDBService = groupDBService;

            Groups = new ObservableCollection<Group>();

            SetCategories();

            SelectGroupCommand = new DelegateCommand<Group>(async (group) =>
            {
                if (group.BannedMembers.Contains(UserObserver.Document.Id))
                {
                    DisplayUserIsBannedError();
                    return;
                }

                try
                {
                    await groupDBService.JoinGroupAsync(group.Id, UserObserver.Document.Id);
                    CloseCommand.Execute(null);
                }
                catch (Exception)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                }
            });

            SelectCategoryCommand = new DelegateCommand<int?>(selectedIndex =>
            {
                if (!selectedIndex.HasValue)
                    return;

                var category = (GroupCategory)selectedIndex.Value;
                Groups = new ObservableCollection<Group>(GetGroupsFromCategory(category));
            });

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public List<string> Categories { get; private set; }

        public ObservableCollection<Group> Groups { get; private set; }

        public LayoutState CurrentState { get; private set; }

        public ICommand CloseCommand { get; private set; }

        public ICommand SelectCategoryCommand { get; private set; }

        public ICommand SelectGroupCommand { get; private set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            CurrentState = LayoutState.Loading;

            var user = UserObserver.Document;

            allGroups = (await groupDBService.GetAllGroups()).ToList();
            allGroups.RemoveAll(g => user.JoinedGroups.Contains(g.Id) || user.OwnedGroups.Contains(g.Id));

            CurrentState = LayoutState.None;
        }

        private void SetCategories()
        {
            Categories = Enum
                .GetValues(typeof(GroupCategory))
                .Cast<GroupCategory>()
                .Select(c => ConvertCategoryToString(c, true))
                .ToList();
        }

        private string ConvertCategoryToString(GroupCategory category, bool addSpacesBeforeCapitals)
        {
            var categoryString = category.ToString();

            if (addSpacesBeforeCapitals)
                categoryString = categoryString.AddSpaceBeforeCapitalLetters();

            return categoryString;
        }

        private IEnumerable<Group> GetGroupsFromCategory(GroupCategory category)
        {
            return allGroups.Where(g => g.Category == category);
        }

        private void DisplayUserIsBannedError()
        {
            DialogExtensions.DisplayMessage(DialogService, "Banned!", "You have been banned from this group.");
        }
    }
}
