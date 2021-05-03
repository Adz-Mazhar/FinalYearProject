using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Group
{
    public interface IGroupDBService
    {
        Task AddGroupAsync(Models.Group group, string newId = null);
        Task BanUserFromGroup(string groupId, string userId);
        Task<IList<Models.Group>> GetAllGroups();
        Task<Models.Group> GetGroupAsync(string groupId);
        Task<IList<Models.Group>> GetGroupsAsync(IEnumerable<string> groupIds);
        Task JoinGroupAsync(string groupId, string userId);
        Task LeaveGroupAsync(string groupId, string userId);
        IDisposable ObserveGroup(string groupId, Action<Models.Group> onGroupModified);
        Task UpdateGroupDescriptionAsync(string groupId, string newGroupDescription);
        Task UpdateGroupNameAsync(string groupId, string newGroupName);
    }
}