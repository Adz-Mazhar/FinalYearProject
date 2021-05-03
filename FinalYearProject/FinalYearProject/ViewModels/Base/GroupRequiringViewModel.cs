using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace FinalYearProject.ViewModels.Base
{
    public class GroupRequiringViewModel : BaseViewModel
    {
        protected IDocumentObserver<Group> GroupObserver { get; init; }

        public GroupRequiringViewModel(INavigationService navigationService,
                                  IDialogService dialogService,
                                  IDocumentObserver<User> userObserver,
                                  IDocumentObserver<Group> groupObserver)
            : base(navigationService, dialogService, userObserver)
        {
            GroupObserver = groupObserver;
        }
    }
}
