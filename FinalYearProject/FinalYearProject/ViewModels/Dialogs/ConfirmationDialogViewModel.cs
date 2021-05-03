using FinalYearProject.Dialogs;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Dialogs
{
    public class ConfirmationDialogViewModel : BaseDialogViewModel
    {
        public ConfirmationDialogViewModel()
        {
            ChooseOptionCommand = new DelegateCommand<ConfirmationDialogOptions>(choice =>
            {
                OnRequestClose(new DialogParameters
                {
                    { "choice",  choice }
                });
            });
        }

        public string Title { get; set; } = "";

        public ICommand ChooseOptionCommand { get; private set; }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
        }
    }
}
