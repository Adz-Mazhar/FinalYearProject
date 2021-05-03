using Plugin.CloudFirestore.Attributes;
using Plugin.CloudFirestore.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.Models
{
    public record Group
    {
        public Group()
        {
        }

        public Group(string name, string description, string owner, GroupCategory category, string code)
        {
            Name = name;
            Description = description;
            Owner = owner;
            Category = category;
            Code = code;
            Members = new List<string>();
            BannedMembers = new List<string>();
        }

        [Id]
        public string Id { get; init; }

        public string Code { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        [DocumentConverter(typeof(EnumStringConverter))]
        public GroupCategory Category { get; init; }

        public string Owner { get; init; }

        public List<string> Members { get; init; }

        public List<string> BannedMembers { get; init; }

        public static string GenerateRandomCode(int length)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable
                .Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}