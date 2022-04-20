using Prism.Services.Dialogs; 
using System.Threading.Tasks;

namespace AppFramework.Services.Dialog
{
    public class AppDialogService : IAppDialogService
    {
        private readonly IDialogService dialogService;

        public AppDialogService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public bool Question(string title, string message)
        {
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("Title", title);
            dialogParameters.Add("Message", message);

            var dialogResult = dialogService.ShowViewDialog("MessageView", dialogParameters);
            return dialogResult.Result == ButtonResult.OK;
        }

        public async Task Show(string title, string message)
        {
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("Title", title);
            dialogParameters.Add("Message", message);
            var dialogResult = dialogService.ShowViewDialog("MessageView", dialogParameters);
            await Task.CompletedTask;
        }
    }
}
