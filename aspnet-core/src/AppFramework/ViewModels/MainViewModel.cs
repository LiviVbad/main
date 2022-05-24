﻿using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Models;
using AppFramework.Services;
using Prism.Commands;
using Prism.Regions;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        public MainViewModel(
            IThemeService themeService,
            IRegionManager regionManager,
            IApplicationService appService)
        {
            this.appService = appService;
            this.themeService = themeService;
            this.regionManager = regionManager;
             
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate); 
            SetThemeModeCommand = new DelegateCommand(themeService.SetThemeMode);
            SetThemeCommand = new DelegateCommand<ThemeItem>(arg => themeService.SetTheme(arg.DisplayName));
        }

        #region 字段/属性

        private bool initialize;
        public IThemeService themeService { get; set; }
        public IApplicationService appService { get; init; }
        private readonly IRegionManager regionManager;
        public DelegateCommand SetThemeModeCommand { get; }
        public DelegateCommand<ThemeItem> SetThemeCommand { get; }
        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; }

        #endregion

        #region 方法

        public void Execute(string arg)
        {
            switch (arg)
            {
                case "ChangeProfilePhoto": appService.ChangeProfilePhoto(); break;
                case "ShowProfilePhoto": appService.ShowProfilePhoto(); break;
            }
        }

        #endregion

        public void Navigate(NavigationItem navigationItem)
        {
            if (navigationItem == null) return;

            Navigate(navigationItem.PageViewName);
        }

        private void Navigate(string pageName)
        {
            regionManager.Regions[AppRegionManager.Main].RequestNavigate(pageName, NavigateionCallBack);
        }

        private void NavigateionCallBack(NavigationResult navigationResult)
        {
            if (navigationResult.Result != null && !(bool)navigationResult.Result)
            { }
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            if (initialize) return;

            await appService.GetApplicationInfo();
            Navigate(AppViewManager.Dashboard);
            initialize = true;
        }
    }
}