using AppFramework.Shared;
using AppFramework.Shared.Services.Signalr;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        private readonly ISignalrService signalr;

        public FriendsChatViewModel(ISignalrService signalr)
        {
            this.signalr=signalr;
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await signalr.StartAsync();
        }
    }
}
