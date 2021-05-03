using FinalYearProject.Extensions;
using Plugin.CloudFirestore.Reactive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FinalYearProject.Services.Database.User
{
    public class UserDBService : BaseDBService, IUserDBService
    {
        public async Task AddUserAsync(Models.User user, string newId = null)
        {
            user.ThrowIfNull(nameof(user));

            await AddAsync(user, newId);
        }

        public async Task<Models.User> GetUserAsync(string userId)
        {
            userId.ThrowIfNull(nameof(userId));

            return await GetAsync<Models.User>(userId);
        }

        public async Task<IList<Models.User>> GetUsersAsync(IEnumerable<string> userIds)
        {
            userIds.ThrowIfNull(nameof(userIds));

            return await GetMultipleAsync<Models.User>(userIds);
        }

        public IDisposable ObserveUser(string userId, Action<Models.User> onUserModified)
        {
            userId.ThrowIfNull(nameof(userId));
            onUserModified.ThrowIfNull(nameof(onUserModified));

            var docRef = GetBaseCollectionReference<Models.User>().Document(userId);

            IDisposable listener = docRef
                .AsObservable()
                .Subscribe(snapshot =>
                {
                    onUserModified.Invoke(snapshot.ToObject<Models.User>());
                });

            return listener;
        }

        public async Task UpdateProfileColourAsync(string userId, Color colour)
        {
            userId.ThrowIfNull(nameof(userId));

            await UpdateAsync<Models.User>(userId, nameof(Models.User.ProfileColourHex), colour.ToHex());
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            userId.ThrowIfNull(nameof(userId));

            await UpdateAsync<Models.User>(userId, nameof(Models.User.Username), newUsername);
        }


    }
}