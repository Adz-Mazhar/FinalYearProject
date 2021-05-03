using FinalYearProject.Dialogs;
using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class ChangePasswordPageViewModel : BaseViewModel
    {
        private readonly IAuthService authService;

        public ChangePasswordPageViewModel(INavigationService navigationService,
                                           IDialogService dialogService,
                                           IDocumentObserver<User> userObserver,
                                           IAuthService authService)
            : base(navigationService, dialogService, userObserver)
        {
            this.authService = authService;

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            SavePasswordCommand = new DelegateCommand(
                executeMethod: async () => await ChangePasswordAsync(),
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(NewPassword);
                })
                .ObservesProperty(() => NewPassword);
        }

        public string NewPassword { get; set; }

        public ICommand CloseCommand { get; }

        public ICommand SavePasswordCommand { get; }

        private async Task ChangePasswordAsync()
        {
            try
            {
                await authService.ChangePasswordAsync(NewPassword);
                await NavigationService.GoBackAsync();
            }
            catch (AuthException e)
            {
                if (e.ErrorType is AuthErrorType.WeakPassword)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "Your password must be at least 6 characters long.");
                }
            }
            catch (System.Exception)
            {
                DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
            }
        }
    }
}
