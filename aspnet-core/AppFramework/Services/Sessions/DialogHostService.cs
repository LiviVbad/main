using AppFramework.WindowHost;
using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AppFramework.Services.Dialog
{
    /// <summary>
    /// 对话主机服务
    /// </summary>
    public class DialogHostService : DialogService, IHostDialogService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this._containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialogAsync(string name, IDialogParameters parameters = null, string IdentifierName = "Root")
        {
            var dialogContent = GetDialogContent(name, IdentifierName);

            if (!(dialogContent.DataContext is IHostDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogHostAware interface");

            var eventHandler = GetDialogOpenedEventHandler(viewModel, parameters);

            var dialogResult = await DialogHost.Show(dialogContent, IdentifierName, eventHandler);

            if (dialogResult == null)
                return new DialogResult(ButtonResult.Cancel);

            return (IDialogResult)dialogResult;
        }

        private FrameworkElement GetDialogContent(string name, string IdentifierName = "Root")
        {
            var content = _containerExtension.Resolve<object>(name);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IHostDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogHostAware interface");

            viewModel.IdentifierName = IdentifierName;

            return dialogContent;
        }

        private DialogOpenedEventHandler GetDialogOpenedEventHandler(IHostDialogAware viewModel,
            IDialogParameters parameters)
        {
            DialogOpenedEventHandler eventHandler =
               (sender, eventArgs) =>
               {
                   var _content = eventArgs.Session.Content;
                   if (viewModel is IHostDialogAware aware)
                       aware.OnDialogOpened(parameters);
                   eventArgs.Session.UpdateContent(_content);
               };

            return eventHandler;
        }
    }
}