using FinalYearProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinalYearProject.ViewModels.Helpers
{
    public class GroupCollection : ObservableCollection<Group>
    {
        public string Name { get; private set; }

        public GroupCollection(string name) : this(name, Enumerable.Empty<Group>())
        {
        }

        public GroupCollection(string name, IEnumerable<Group> groups) : base(groups)
        {
            Name = name;
        }
    }
}
