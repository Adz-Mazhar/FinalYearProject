using Prism.Services.Dialogs;

namespace FinalYearProject.Dialogs
{
    public static class DialogExtensions
    {
        public static void DisplayMessage(IDialogService dialogService, string title, string message)
        {
            var navParams = new DialogParameters
            {
                { "title", title },
                { "message", message }
            };

            dialogService.ShowDialog("MessageDialog", navParams);
        }
    }
}
