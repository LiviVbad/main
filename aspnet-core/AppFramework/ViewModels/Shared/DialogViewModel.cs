using Prism.Services.Dialogs;
using System;

namespace AppFramework.ViewModels
{
    public class DialogViewModel : ViewModelBase, IDialogAware
    {
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        public void OnDialogClosed(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}