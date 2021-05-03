using FinalYearProject.Models;
using FinalYearProject.ViewModels.Helpers;

namespace FinalYearProject.ViewModels.Pages
{
    public class NewMessageEventArgs
    {
        public MessageCollection MessageGroup { get; set; }

        public Message Message { get; set; }
    }
}
