using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace FinalYearProject.ViewModels.Base
{
    public abstract class BaseViewModel : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; init; }
        protected IDialogService DialogService { get; init; }
        protected IDocumentObserver<User> UserObserver { get; init; }

        public string Title { get; set; } = "";

        public bool IsBusy { get; set; }

        public BaseViewModel(INavigationService navigationService, IDialogService dialogService, IDocumentObserver<User> userObserver)
        {
            NavigationService = navigationService;
            DialogService = dialogService;
            UserObserver = userObserver;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }
    }
}