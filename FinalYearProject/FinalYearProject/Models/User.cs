using Plugin.CloudFirestore.Attributes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FinalYearProject.Models
{
    public record User
    {
        public User()
        {
        }

        public User(string username, Color profileColour = default)
        {
            Username = username;
            ProfileColourHex = profileColour.ToHex();
            OwnedGroups = new List<string>();
            JoinedGroups = new List<string>();
            JoinedActivities = new List<string>();
            Posts = new List<string>();
        }

        [Id]
        public string Id { get; init; }

        public string Username { get; init; }

        public string ProfileColourHex { get; init; }

        public List<string> OwnedGroups { get; init; }

        public List<string> JoinedGroups { get; init; }

        public List<string> JoinedActivities { get; init; }

        public List<string> Posts { get; init; }
    }
}