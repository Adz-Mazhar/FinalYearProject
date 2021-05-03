using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System;

namespace FinalYearProject.Models
{
    public record Activity
    {
        public Activity()
        {
        }

        public Activity(string title, User creator, DateTime endsAt)
        {
            Title = title;
            Creator = creator?.Id;
            CreatorInfo = new UserDisplayInfo(creator);
            EndsAt = endsAt;
        }

        [Id]
        public string Id { get; init; }

        public string Title { get; init; }

        public string Creator { get; init; }

        public UserDisplayInfo CreatorInfo { get; init; }

        public int FollowerCount { get; set; }

        public int CompletedCount { get; set; }

        [ServerTimestamp]
        public Timestamp CreatedAt { get; init; }

        public DateTime EndsAt { get; init; }

        [Ignored]
        public bool IsFollowedByUser { get; set; }
    }
}
