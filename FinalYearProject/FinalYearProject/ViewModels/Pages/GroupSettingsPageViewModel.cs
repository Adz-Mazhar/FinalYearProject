using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class GroupSettingsPageViewModel : GroupRequiringViewModel
    {
        public GroupSettingsPageViewModel(INavigationService navigationService,
                                          IDialogService dialogService,
                                          IDocumentObserver<User> userObserver,
                                          IDocumentObserver<Group> groupObserver)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            Title = "Settings";

            ChangeGroupNameCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ChangeGroupNamePage");
            });

            ChangeGroupDescriptionCommmand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ChangeGroupDescriptionPage");
            });
        }

        public ICommand ChangeGroupNameCommand { get; private set; }

        public ICommand ChangeGroupDescriptionCommmand { get; private set; }
    }
}
