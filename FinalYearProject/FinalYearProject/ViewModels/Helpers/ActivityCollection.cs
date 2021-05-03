using FinalYearProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinalYearProject.ViewModels.Helpers
{
    public class ActivityCollection : ObservableCollection<Activity>
    {
        public string Name { get; set; }

        public ActivityCollection(string name) : this(name, Enumerable.Empty<Activity>())
        {
        }

        public ActivityCollection(string name, IEnumerable<Activity> activities) : base(activities)
        {
            Name = name;
        }
    }
}
