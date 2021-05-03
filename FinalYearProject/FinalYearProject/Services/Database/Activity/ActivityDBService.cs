using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database.Transactions;
using Plugin.CloudFirestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Activity
{
    public class ActivityDBService : BaseDBService, IActivityDBService
    {
        private static readonly string ActivitiesCollectionName = "Activities";

        public ActivityDBService()
        {
            RegisterTransactionTasks();
        }

        private enum ActivityTransactionType { Follow, Unfollow, Complete }

        private enum UserTransactionType { AddActivity, RemoveActivity }

        public async Task AddActivityAsync(Models.Activity activity,
                                           string groupId,
                                           string newId = null)
        {
            activity.ThrowIfNull(nameof(activity));
            groupId.ThrowIfNull(nameof(groupId));

            await AddAsync(activity, newId, ActivitiesCollectionReference(groupId));
        }

        public async Task CompleteActivityAsync(string groupId, string activityId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            activityId.ThrowIfNull(nameof(activityId));
            userId.ThrowIfNull(nameof(userId));

            await RunUserTransactionAsync(userId, UserTransactionType.RemoveActivity, new object[] { activityId });
            await RunActivityTransactionAsync(groupId, activityId, ActivityTransactionType.Complete);
        }

        public async Task<IList<Models.Activity>> GetActivitiesAsync(string groupId, string orderedByField)
        {
            groupId.ThrowIfNull(nameof(groupId));

            return await GetAllAsync<Models.Activity>(ActivitiesCollectionReference(groupId).OrderBy(orderedByField));
        }

        public async Task DeleteActivityAsync(string groupId, string activityId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            activityId.ThrowIfNull(nameof(activityId));

            var query = await GetBaseCollectionReference<Models.User>()
                .WhereArrayContains(nameof(Models.User.JoinedActivities), activityId)
                .GetAsync();

            foreach (var user in query.Documents)
            {
                await RunUserTransactionAsync(user.Id, UserTransactionType.RemoveActivity, new object[] { activityId });
            }

            await DeleteAsync<Models.Activity>(activityId, ActivitiesCollectionReference(groupId));
        }

        public async Task FollowActivityAsync(string groupId, string activityId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            activityId.ThrowIfNull(nameof(activityId));
            userId.ThrowIfNull(nameof(userId));

            await RunUserTransactionAsync(userId, UserTransactionType.AddActivity, new object[] { activityId });
            await RunActivityTransactionAsync(groupId, activityId, ActivityTransactionType.Follow);
        }

        public async Task UnfollowActivityAsync(string groupId, string activityId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            activityId.ThrowIfNull(nameof(activityId));
            userId.ThrowIfNull(nameof(userId));

            await RunUserTransactionAsync(userId, UserTransactionType.RemoveActivity, new object[] { activityId });
            await RunActivityTransactionAsync(groupId, activityId, ActivityTransactionType.Unfollow);
        }

        private ICollectionReference ActivitiesCollectionReference(string groupId)
        {
            return GetBaseCollectionReference<Models.Group>()
                .Document(groupId)
                .Collection(ActivitiesCollectionName);
        }

        private void RegisterTransactionTasks()
        {
            RegisterMultipleTransactionTasks(new List<(string, ITransactionTask)>
            {
                (nameof(ActivityTransactionType.Follow), new TransactionTask<Models.Activity>
                {
                    Action = activity => activity.FollowerCount++,
                }),

                (nameof(ActivityTransactionType.Unfollow), new TransactionTask<Models.Activity>
                {
                    Action = activity => activity.FollowerCount--,
                }),

                (nameof(ActivityTransactionType.Complete), new TransactionTask<Models.Activity>
                {
                    Action = activity =>
                    {
                        activity.CompletedCount++;
                        activity.FollowerCount--;
                    }
                }),

                (nameof(UserTransactionType.AddActivity), new TransactionTask<Models.User, string>
                {
                    Action = (user, activityId) => user.JoinedActivities.Add(activityId)
                }),

                (nameof(UserTransactionType.RemoveActivity), new TransactionTask<Models.User, string>
                {
                    Action = (user, activityId) => user.JoinedActivities.Remove(activityId)
                }),
            });
        }

        private async Task RunActivityTransactionAsync(string groupId, string activityId, ActivityTransactionType transactionType, object[] parameters = null)
        {
            await RunTransactionAsync<Models.Activity>(
                ActivitiesCollectionReference(groupId).Document(activityId),
                transactionType.ToString(),
                parameters);
        }

        private async Task RunUserTransactionAsync(string userId, UserTransactionType transactionType, object[] parameters = null)
        {
            await RunTransactionAsync<Models.User>(
                GetBaseCollectionReference<Models.User>().Document(userId),
                transactionType.ToString(),
                parameters);
        }
    }
}
