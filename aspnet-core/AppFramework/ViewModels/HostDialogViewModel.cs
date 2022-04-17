using AppFramework.Services;
using AppFramework.WindowHost;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public abstract class HostDialogViewModel : ViewModelBase, IDialogHostAware
    {
        public HostDialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        protected virtual void Cancel()
        {
            DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.No));
        }

        protected virtual void Save()
        {
            DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.OK));
        }

        protected virtual void Save(object value)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", value);

            DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.OK, param));
        }

        protected virtual void Save(DialogParameters param)
        {
            DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.OK, param));
        }

        public string IdentifierName { get; set; }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}