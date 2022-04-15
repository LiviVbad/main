using AppFramework.Services;
using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System; 
using System.Threading.Tasks;
using System.Windows;

namespace AppFramework.WindowHost
{
    /// <summary>
    /// 对话主机服务
    /// </summary>
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this._containerExtension = containerExtension;
        }

        public void ShowDialog(string name, Action<IDialogResult> callback)
        {
            var content = _containerExtension.Resolve<object>(name);
            if (!(content is Window dialogContent))
                throw new NullReferenceException("A dialog's content must be a Window");

            if (dialogContent is Window view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            if (dialogContent is IDialogWindow dialogWindow)
                ConfigureDialogWindowEvents(dialogWindow, callback);

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(viewModel, d => d.OnDialogOpened(null));

            dialogContent.ShowDialog();
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters = null, string IdentifierName = "Root")
        {
            if (parameters == null)
                parameters = new DialogParameters();

            var content = _containerExtension.Resolve<object>(name);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogHostAware interface");

            viewModel.IdentifierName = IdentifierName;

            DialogOpenedEventHandler eventHandler =
                (sender, eventArgs) =>
           {
               var _content = eventArgs.Session.Content;
               if (viewModel is IDialogHostAware aware)
                   aware.OnDialogOpened(parameters);
               eventArgs.Session.UpdateContent(_content);
           };

            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.IdentifierName, eventHandler);
        }
    }
}
