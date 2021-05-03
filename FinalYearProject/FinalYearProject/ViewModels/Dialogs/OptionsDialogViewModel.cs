using FinalYearProject.Dialogs;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Dialogs
{
    public class OptionsDialogViewModel : BaseDialogViewModel
    {
        public OptionsDialogViewModel()
        {
            ChooseOptionCommand = new DelegateCommand<DialogOptionBase>(choice =>
            {
                OnRequestClose(new DialogParameters
                {
                    { "choice",  choice }
                });
            });
        }

        public List<DialogOptionBase> Options { get; private set; }

        public ICommand ChooseOptionCommand { get; private set; }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Options = parameters.GetValue<List<DialogOptionBase>>("options");

            Options ??= new List<DialogOptionBase>();
        }
    }
}
