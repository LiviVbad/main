using AppFramework.Common.ViewModels;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace AppFramework.Shared.ViewModels
{
    public class DialogViewModel : DialogModelBase, IDialogAware
    {
        public event Action<IDialogParameters> RequestClose;

        public virtual bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public override void Cancel()
        {
            DialogParameters param = new DialogParameters();
            param.Add("DialogResult", false);
            RequestClose(param);
        }

        public override void OnSave()
        {
            Save(true);
        }

        public void Save(bool Result = true)
        {
            RequestClose(new DialogParameters() { { "DialogResult", Result } });
        }

        public void Save(DialogParameters param) => RequestClose(param);

        public virtual void OnDialogOpened(IDialogParameters parameters) { }
    }
}