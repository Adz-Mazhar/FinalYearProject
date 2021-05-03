using FinalYearProject.Dialogs;
using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IAuthService authService;

        public LoginPageViewModel(INavigationService navigationService,
                                  IDialogService dialogService,
                                  IDocumentObserver<User> userObserver,
                                  IAuthService authService)
            : base(navigationService, dialogService, userObserver)
        {
            this.authService = authService;

            LoginCommand = new DelegateCommand(
                executeMethod: async () => await SignInAsync(),
                canExecuteMethod: () =>
                {
                    var strings = new[] { Email, Password };
                    return strings.All(s => !string.IsNullOrEmpty(s));
                })
                .ObservesProperty(() => Email)
                .ObservesProperty(() => Password);

            SignUpCommand = new DelegateCommand(async () =>
                await NavigationService.NavigateAsync("SignUpPage"));
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public ICommand SignUpCommand { get; }

        private async Task SignInAsync()
        {
            string userId = null;

            try
            {
                userId = await authService.SignInAsync(Email.Trim(), Password);
            }
            catch (AuthException e)
            {
                switch (e.ErrorType)
                {
                    case AuthErrorType.InvalidEmail:
                        DisplayError("Please enter a valid email address.");
                        break;

                    case AuthErrorType.NotRegistered:
                        DisplayError("This email address has not been registered. Sign up to register.");
                        break;

                    case AuthErrorType.WrongPassword:
                        DisplayError("The password you have entered is incorrect.");
                        break;
                }
            }
            catch (System.Exception)
            {
                DisplayError("An error occured. Please try again.");
            }

            if (userId is not null)
            {
                await ((App)App.Current).LoadMainApp();
            }
        }

        private void DisplayError(string message)
        {
            DialogExtensions.DisplayMessage(DialogService, "Error!", message);
        }
    }
}