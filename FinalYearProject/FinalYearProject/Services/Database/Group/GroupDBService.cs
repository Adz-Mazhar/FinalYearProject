using FinalYearProject.Extensions;
using FinalYearProject.Services.Database.Transactions;
using Plugin.CloudFirestore.Reactive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalYearProject.Services.Database.Group
{
    public class GroupDBService : BaseDBService, IGroupDBService
    {
        public GroupDBService()
        {
            RegisterTransactionTasks();
        }

        private enum GroupTransactionType { AddMember, RemoveMember, BanMember }

        private enum UserTransactionType { AddJoinedGroup, RemoveJoinedGroup, AddOwnedGroup, RemoveOwnedGroup }

        public async Task AddGroupAsync(Models.Group group,
                                        string newId = null)
        {
            group.ThrowIfNull(nameof(group));

            var docRef = await AddAsync(group, newId);

            newId ??= docRef.Id;
            await RunUserTransactionAsync(group.Owner, UserTransactionType.AddOwnedGroup, new object[] { newId });
        }

        public async Task BanUserFromGroup(string groupId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            userId.ThrowIfNull(nameof(userId));

            await RunGroupTransactionAsync(groupId, GroupTransactionType.BanMember, new object[] { userId });
            await RunUserTransactionAsync(userId, UserTransactionType.RemoveJoinedGroup, new object[] { groupId });
        }

        public async Task<IList<Models.Group>> GetAllGroups()
        {
            return await GetAllAsync<Models.Group>();
        }

        public async Task<Models.Group> GetGroupAsync(string groupId)
        {
            groupId.ThrowIfNull(nameof(groupId));

            return await GetAsync<Models.Group>(groupId);
        }

        public async Task<IList<Models.Group>> GetGroupsAsync(IEnumerable<string> groupIds)
        {
            groupIds.ThrowIfNull(nameof(groupIds));

            return await GetMultipleAsync<Models.Group>(groupIds);
        }

        public async Task JoinGroupAsync(string groupId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            userId.ThrowIfNull(nameof(userId));

            await RunGroupTransactionAsync(groupId, GroupTransactionType.AddMember, new object[] { userId });
            await RunUserTransactionAsync(userId, UserTransactionType.AddJoinedGroup, new object[] { groupId });
        }

        public async Task LeaveGroupAsync(string groupId, string userId)
        {
            groupId.ThrowIfNull(nameof(groupId));
            userId.ThrowIfNull(nameof(userId));

            await RunGroupTransactionAsync(groupId, GroupTransactionType.RemoveMember, new object[] { userId });
            await RunUserTransactionAsync(userId, UserTransactionType.RemoveJoinedGroup, new object[] { groupId });
        }

        public async Task UpdateGroupNameAsync(string groupId, string newGroupName)
        {
            groupId.ThrowIfNull(nameof(groupId));

            await UpdateAsync<Models.Group>(groupId, nameof(Models.Group.Name), newGroupName);
        }

        public async Task UpdateGroupDescriptionAsync(string groupId, string newGroupDescription)
        {
            groupId.ThrowIfNull(nameof(groupId));

            await UpdateAsync<Models.Group>(groupId, nameof(Models.Group.Description), newGroupDescription);
        }

        public IDisposable ObserveGroup(string groupId, Action<Models.Group> onGroupModified)
        {
            groupId.ThrowIfNull(nameof(groupId));
            onGroupModified.ThrowIfNull(nameof(onGroupModified));

            var docRef = GetBaseCollectionReference<Models.Group>().Document(groupId);

            IDisposable listener = docRef
                .AsObservable()
                .Subscribe(snapshot =>
                {
                    onGroupModified.Invoke(snapshot.ToObject<Models.Group>());
                });

            return listener;
        }

        private void RegisterTransactionTasks()
        {
            RegisterMultipleTransactionTasks(new List<(string, ITransactionTask)>
            {
                (nameof(GroupTransactionType.AddMember), new TransactionTask<Models.Group, string>
                {
                    Action = (group, userId) => group.Members.Add(userId)
                }),

                (nameof(GroupTransactionType.RemoveMember), new TransactionTask<Models.Group, string>
                {
                    Action = (group, userId) => group.Members.Remove(userId)
                }),

                (nameof(GroupTransactionType.BanMember), new TransactionTask<Models.Group, string>
                {
                    Action = (group, userId) =>
                    {
                        group.BannedMembers.Add(userId);
                        group.Members.Remove(userId);
                    }
                }),

                (nameof(UserTransactionType.AddJoinedGroup), new TransactionTask<Models.User, string>
                {
                    Action = (user, groupId) => user.JoinedGroups.Add(groupId)
                }),

                (nameof(UserTransactionType.RemoveJoinedGroup), new TransactionTask<Models.User, string>
                {
                    Action = (user, groupId) => user.JoinedGroups.Remove(groupId)
                }),

                (nameof(UserTransactionType.AddOwnedGroup), new TransactionTask<Models.User, string>
                {
                    Action = (user, groupId) => user.OwnedGroups.Add(groupId)
                }),

                (nameof(UserTransactionType.RemoveOwnedGroup), new TransactionTask<Models.User, string>
                {
                    Action = (user, groupId) => user.OwnedGroups.Remove(groupId)
                }),
            });
        }

        private async Task RunGroupTransactionAsync(string groupId, GroupTransactionType transactionType, object[] parameters = null)
        {
            await RunTransactionAsync<Models.Group>(
                GetBaseCollectionReference<Models.Group>().Document(groupId),
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