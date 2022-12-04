using AppFramework.Authorization.Users;
using AppFramework.Shared;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class CreateLinkedAccountViewModel : HostDialogViewModel
    {
        public CreateLinkedAccountViewModel(IUserLinkAppService appService)
        {
            this.appService = appService;
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string tenancyName;

        public string TenancyName
        {
            get { return tenancyName; }
            set { tenancyName = value; RaisePropertyChanged(); }
        }

        private string password;
        private readonly IUserLinkAppService appService;

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }

        public override async Task Save()
        {
            await WebRequest.Execute(() => appService.LinkToUser(new Authorization.Users.Dto.LinkToUserInput()
            {
                TenancyName = TenancyName,
                UsernameOrEmailAddress = UserName,
                Password = Password
            }), base.Save);
        }

        public override void OnDialogOpened(IDialogParameters parameters) { }
    }
}
