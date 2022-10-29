using AppFramework.Services;
using AppFramework.WindowHost;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace AppFramework.Shared
{
    public abstract class HostDialogViewModel : ViewModelBase, IHostDialogAware
    {
        public string Title { get; set; }

        public string IdentifierName { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public HostDialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public virtual void Cancel()
        {
            DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.No));
        }

        public virtual void Save()
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

        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}