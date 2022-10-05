﻿using AppFramework.ApiClient;
using Prism.Commands;
using Prism.Regions.Navigation;
using AppFramework.Shared.Services;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Models;
using AppFramework.Shared.Core; 

namespace AppFramework.Shared.ViewModels
{
    public class MainViewModel : RegionViewModel
    {
        public MainViewModel(
            IRegionNavigateService regionService,
            IAccountService accountService,
            IApplicationContext applicationContext,
            IApplicationService appService,
            IMessenger messenger)
        {
            this.regionService = regionService;
            this.accountService = accountService;
            this.applicationContext = applicationContext;
            this.appService = appService;
            this.messenger = messenger;
            ExecuteCommand = new DelegateCommand<string>(Execute);

            messenger.Sub(AppMessengerKeys.LanguageRefresh, appService.RefreshAuthMenus);
        }

        #region 字段/属性

        private readonly IRegionNavigateService regionService;
        private readonly IAccountService accountService;
        private readonly IApplicationContext applicationContext;
        private readonly IMessenger messenger;

        public IApplicationService appService { get; }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #endregion 字段/属性

        #region 方法

        public async void Execute(string arg)
        {
            switch (arg)
            {
                case "ShowProfilePhoto": await appService.ShowProfilePhoto(); break;
                case "Home": SetDefaultPage(); break;
                case "MyProfile": await appService.ShowMyProfile(); break;
            }
        }

        public override async void OnNavigatedTo(INavigationContext navigationContext)
        {
            if (applicationContext.LoginInfo == null) return;
            //设置应用程序信息
            await appService.GetApplicationInfo();
            //初始化系统首页
            SetDefaultPage();
        }

        public void Navigate(NavigationItem item)
        {
            regionService.Navigate(AppRegionManager.Main, item.PageViewName);
        }

        private void SetDefaultPage()
        {
            regionService.Navigate(AppRegionManager.Main, AppViewManager.Dashboard);
        }

        #endregion
    }
}