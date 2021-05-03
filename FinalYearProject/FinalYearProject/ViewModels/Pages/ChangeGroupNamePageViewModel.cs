using FinalYearProject.Dialogs;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class ChangeGroupNamePageViewModel : GroupRequiringViewModel
    {
        public ChangeGroupNamePageViewModel(INavigationService navigationService,
                                            IDialogService dialogService,
                                            IDocumentObserver<User> userObserver,
                                            IDocumentObserver<Group> groupObserver,
                                            IGroupDBService groupDBService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            SaveCommand = new DelegateCommand(
                executeMethod: async () =>
                {
                    try
                    {
                        await groupDBService.UpdateGroupNameAsync(GroupObserver.Document.Id, NewGroupName);
                        CloseCommand.Execute(null);
                    }
                    catch (System.Exception)
                    {
                        DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                    }
                },
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(NewGroupName);
                })
                .ObservesProperty(() => NewGroupName);
        }

        public string NewGroupName { get; set; }

        public ICommand CloseCommand { get; }

        public ICommand SaveCommand { get; }
    }
}
