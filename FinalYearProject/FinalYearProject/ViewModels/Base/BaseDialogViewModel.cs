using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Base
{
    public class BaseDialogViewModel : BindableBase, IDialogAware
    {
        public BaseDialogViewModel()
        {
            CloseCommand = new DelegateCommand(() => OnRequestClose(null));
        }

        public event Action<IDialogParameters> RequestClose;

        public ICommand CloseCommand { get; private set; }

        public virtual bool CanCloseDialog() => true;

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
        }

        protected virtual void OnRequestClose(IDialogParameters parameters)
        {
            var handler = RequestClose;
            handler?.Invoke(parameters);
        }
    }
}
