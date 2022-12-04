using AppFramework.Services;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Prism.Ioc; 
using AppFramework.Shared.Services;

namespace AppFramework.Shared
{
    public abstract class HostDialogViewModel : ViewModelBase, IHostDialogAware
    {
        public string Title { get; set; }

        public string IdentifierName { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        private IHostDialogService dialogService;

        public HostDialogViewModel()
        {
            SaveCommand = new DelegateCommand(async () => await Save());
            CancelCommand = new DelegateCommand(Cancel);

            dialogService=ContainerLocator.Container.Resolve<IHostDialogService>();
        }

        public virtual void Cancel()
        {
            dialogService.Close(IdentifierName, new DialogResult(ButtonResult.No));
        }

        public virtual async Task Save()
        {
            dialogService.Close(IdentifierName, new DialogResult(ButtonResult.OK));
            await Task.CompletedTask;
        }

        protected virtual void Save(object value)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", value);

            //DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.OK, param));
        }

        protected virtual void Save(DialogParameters param)
        {
            //DialogHost.Close(IdentifierName, new DialogResult(ButtonResult.OK, param));
        }

        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}