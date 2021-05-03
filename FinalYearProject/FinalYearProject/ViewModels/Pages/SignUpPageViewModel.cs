using FinalYearProject.Dialogs;
using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.User;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class SignUpPageViewModel : BaseViewModel
    {
        private readonly IAuthService authService;
        private readonly IUserDBService userDBService;

        public SignUpPageViewModel(INavigationService navigationService,
                                   IDialogService dialogService,
                                   IDocumentObserver<User> userObserver,
                                   IAuthService authService,
                                   IUserDBService userDBService)
            : base(navigationService, dialogService, userObserver)
        {
            this.authService = authService;
            this.userDBService = userDBService;

            SignUpCommand = new DelegateCommand(
                executeMethod: async () => await SignUpAsync(),
                canExecuteMethod: () =>
                {
                    return !(string.IsNullOrWhiteSpace(Username)
                             && string.IsNullOrWhiteSpace(Password)
                             && string.IsNullOrWhiteSpace(Email));
                })
                .ObservesProperty(() => Username)
                .ObservesProperty(() => Password)
                .ObservesProperty(() => Email);

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICommand SignUpCommand { get; }

        public ICommand CloseCommand { get; }

        private async Task SignUpAsync()
        {
            string userId = null;
            try
            {
                userId = await authService.SignUpAsync(Email, Password);
            }
            catch (AuthException e)
            {
                switch (e.ErrorType)
                {
                    case AuthErrorType.InvalidEmail:
                        DisplayError("Please enter a valid email address.");
                        break;

                    case AuthErrorType.UserCollision:
                        DisplayError("This email has already been registered.");
                        break;

                    case AuthErrorType.WeakPassword:
                        DisplayError("Your password must be at least 6 characters long.");
                        break;
                }
            }
            catch (System.Exception)
            {
                DisplayError("An error occured. Please try again.");
            }

            if (userId is not null)
            {
                User newUser = new(Username);
                await userDBService.AddUserAsync(newUser, userId);

                await NavigationService.GoBackAsync();
            }
        }

        private void DisplayError(string message)
        {
            DialogExtensions.DisplayMessage(DialogService, "Error!", message);
        }
    }
}