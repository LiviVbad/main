using Prism.Commands; 

namespace AppFramework.Common.ViewModels
{
    public abstract class BaseDialogViewModel : ViewModelBase
    {
        public BaseDialogViewModel()
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