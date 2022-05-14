using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services; 
using AppFramework.Models;
using Prism.Commands;
using Prism.Regions;

namespace AppFramework.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        public MainViewModel(
            IRegionManager regionManager,
            IApplicationService appService)
        {
            this.appService = appService;
            this.regionManager = regionManager;

            ExecuteCommand = new DelegateCommand<string>(Execute);
            SetThemeCommand = new DelegateCommand<ThemeItem>(
                arg => appService.SetTheme(arg.DisplayName));
            SetThemeModeCommand = new DelegateCommand(appService.SetThemeMode);
        }
         
        #region 字段/属性

        private readonly IRegionManager regionManager;
        public IApplicationService appService { get; init; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand SetThemeModeCommand { get; }
        public DelegateCommand<ThemeItem> SetThemeCommand { get; }
        
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
            if (navigationItem.Items?.Count > 0) return;

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
         
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            appService.GetApplicationInfo();
            Navigate(AppViewManager.Dashboard);
        }
    }
}