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
    public class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IDocumentObserver<User> userObserver,
                                    IEventAggregator eventAggregator)
            : base(navigationService, dialogService, userObserver)
        {
            Title = "Profile";

            eventAggregator.GetEvent<UserChangedEvent>().Subscribe(
                Refresh,
                ThreadOption.UIThread,
                false);

            SeePostsCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("UserPostsPage");
            });
        }

        public User User { get; private set; }

        public ICommand SeePostsCommand { get; private set; }

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