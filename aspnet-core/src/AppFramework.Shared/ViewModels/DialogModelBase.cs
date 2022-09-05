using Prism.Commands; 

namespace AppFramework.Shared
{
    public abstract class DialogModelBase : ViewModelBase
    {
        public DialogModelBase()
        {
            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public abstract void Cancel();

        public abstract void OnSave();
    }
}