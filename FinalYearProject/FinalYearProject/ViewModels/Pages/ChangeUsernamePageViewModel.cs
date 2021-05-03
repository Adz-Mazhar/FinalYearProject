using FinalYearProject.Dialogs;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.User;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class ChangeUsernamePageViewModel : BaseViewModel
    {
        public ChangeUsernamePageViewModel(INavigationService navigationService,
                                           IDialogService dialogService,
                                           IDocumentObserver<User> userObserver,
                                           IUserDBService userDBService)
            : base(navigationService, dialogService, userObserver)
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
                        await userDBService.UpdateUsernameAsync(UserObserver.Document.Id, NewUsername);
                        CloseCommand.Execute(null);
                    }
                    catch (System.Exception)
                    {
                        DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                    }
                },
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(NewUsername);
                })
                .ObservesProperty(() => NewUsername);
        }

        public string NewUsername { get; set; }

        public ICommand CloseCommand { get; }

        public ICommand SaveCommand { get; }
    }
}
