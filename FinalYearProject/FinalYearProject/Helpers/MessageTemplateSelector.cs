using FinalYearProject.Models;
using FinalYearProject.ViewModels.Helpers;
using Xamarin.Forms;

namespace FinalYearProject.Helpers
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncomingDataTemplate { get; set; }

        public DataTemplate OutgoingDataTemplate { get; set; }

        public DataTemplate SystemDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is not Message message)
            {
                return null;
            }

            return message.Sender == Constants.SystemId
                ? SystemDataTemplate
                : message.IsOwnMessage ? OutgoingDataTemplate : IncomingDataTemplate;
        }
    }
}
