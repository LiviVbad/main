using AppFramework.ApiClient;
using AppFramework.Common.Models;
using AppFramework.ViewModels.Shared;
using Prism.Services.Dialogs; 

namespace AppFramework.ViewModels
{
    public class MyProfileViewModel : HostDialogViewModel
    {
        private readonly IApplicationContext applicationContext;

        public MyProfileViewModel(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        private UserLoginInfoModel userInfo;

        public UserLoginInfoModel UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; RaisePropertyChanged(); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            UserInfo = Map<UserLoginInfoModel>(applicationContext.LoginInfo.User);
        }
    }
}
