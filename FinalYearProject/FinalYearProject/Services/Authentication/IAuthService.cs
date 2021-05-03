using System.Threading.Tasks;

namespace FinalYearProject.Services.Authentication
{
    public interface IAuthService
    {
        Task ChangePasswordAsync(string newPassword);
        string GetUserId();
        bool IsSignedIn();
        Task<string> SignInAsync(string email, string password);
        bool SignOut();
        Task<string> SignUpAsync(string email, string password);
    }
}