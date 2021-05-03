using FinalYearProject.Dialogs;
using FinalYearProject.Events;
using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class UserSettingsPageViewModel : BaseViewModel
    {
        private readonly IAuthService authService;

        public UserSettingsPageViewModel(INavigationService navigationService,
                                         IDialogService dialogService,
                                         IDocumentObserver<User> userObserver,
                                         IEventAggregator eventAggregator,
                                         IAuthService authService)
            : base(navigationService, dialogService, userObserver)
        {
            this.authService = authService;

            Title = "Settings";

            eventAggregator.GetEvent<UserChangedEvent>().Subscribe(
                Refresh,
                ThreadOption.UIThread,
                false);

            SignOutCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.authService.SignOut();
                    await ((App)App.Current).GoToLogin();
                }
                catch (System.Exception)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                }
            });

            ChangeColourCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ColoursPage");
            });

            ChangeUsernameCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ChangeUsernamePage");
            });

            ChangePasswordCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ChangePasswordPage");
            });
        }

        public User User { get; private set; }

        public ICommand SignOutCommand { get; private set; }

        public ICommand ChangeColourCommand { get; private set; }

        public ICommand ChangeUsernameCommand { get; private set; }

        public ICommand ChangePasswordCommand { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            Refresh();
        }

        private void Refresh()
        {
            IsBusy = true;

            User = UserObserver.Document;

            IsBusy = false;
        }
    }
}