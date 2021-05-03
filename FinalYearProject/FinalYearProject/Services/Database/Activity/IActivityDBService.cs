using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Activity
{
    public interface IActivityDBService
    {
        Task AddActivityAsync(Models.Activity activity, string groupId, string newId = null);
        Task CompleteActivityAsync(string groupId, string activityId, string userId);
        Task DeleteActivityAsync(string groupId, string activityId);
        Task FollowActivityAsync(string groupId, string activityId, string userId);
        Task<IList<Models.Activity>> GetActivitiesAsync(string groupId, string orderedByField);
        Task UnfollowActivityAsync(string groupId, string activityId, string userId);
    }
}