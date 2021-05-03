using FinalYearProject.ViewModels.Base;
using Prism.Services.Dialogs;

namespace FinalYearProject.ViewModels.Dialogs
{
    class MessageDialogViewModel : BaseDialogViewModel
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
        }
    }
}
