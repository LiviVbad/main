using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public abstract class DialogViewModel : ViewModelBase, IDialogAware
    {
        public DialogViewModel()
        {
            SaveCommand=new DelegateCommand(OnSave);
            CancelCommand=new DelegateCommand(Cancel);
        }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public event Action<IDialogParameters> RequestClose;

        public virtual bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void Cancel()
        {
            DialogParameters param = new DialogParameters();
            param.Add("DialogResult", false);
            RequestClose(param);
        }

        public void Save(bool Result = true) => RequestClose(new DialogParameters() { { "DialogResult", Result } });

        public void Save(DialogParameters param) => RequestClose(param);

        public virtual void OnSave() => Save(true);

        public virtual void OnDialogOpened(IDialogParameters parameters) { }
    }
}