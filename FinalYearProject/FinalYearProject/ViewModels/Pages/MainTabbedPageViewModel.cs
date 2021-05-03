using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Base;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace FinalYearProject.ViewModels.Pages
{
    public class MainTabbedPageViewModel : BaseViewModel
    {
        public MainTabbedPageViewModel(INavigationService navigationService,
                                       IDialogService dialogService,
                                       IDocumentObserver<User> userObserver)
            : base(navigationService, dialogService, userObserver)
        {
        }
    }
}
