using AppFrameworkDemo.ApiClient;
using AppFramework.Application.Common.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class MyProfileViewModel : DialogViewModel
    {
        private UserLoginInfoModel userInfo;

        public UserLoginInfoModel UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; RaisePropertyChanged(); }
        }


        private readonly IApplicationContext applicationContext;

        public MyProfileViewModel(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            UserInfo = Map<UserLoginInfoModel>(applicationContext.LoginInfo.User);
            base.OnDialogOpened(parameters);
        }
    }
}
