using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FinalYearProject.Services.Database.User
{
    public interface IUserDBService
    {
        Task AddUserAsync(Models.User user, string newId = null);
        Task<Models.User> GetUserAsync(string userId);
        Task<IList<Models.User>> GetUsersAsync(IEnumerable<string> userIds);
        IDisposable ObserveUser(string userId, Action<Models.User> onUserModified);
        Task UpdateProfileColourAsync(string userId, Color colour);
        Task UpdateUsernameAsync(string userId, string newUsername);
    }
}