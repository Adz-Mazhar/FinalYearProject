using FinalYearProject.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinalYearProject.ViewModels.Helpers
{
    public class MessageCollection : ObservableCollection<Message>
    {
        public string Date { get; }

        public MessageCollection(string day) : this(day, Enumerable.Empty<Message>())
        {
        }

        public MessageCollection(string day, IEnumerable<Message> messages) : base(messages)
        {
            Date = day;
        }
    }
}
