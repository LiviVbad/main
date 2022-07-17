using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Account;
using AppFramework.Models;
using AppFramework.Services;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using Syncfusion.Data.Extensions;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppFramework.ViewModels
{
    public class MainTabsViewModel : NavigationViewModel
    {
        public MainTabsViewModel(
            IAccountService accountService,
            IThemeService themeService,
            IRegionManager regionManager,
            IApplicationService appService)
        {
            this.appService = appService;
            this.accountService = accountService;
            this.themeService = themeService;
            this.regionManager = regionManager;

            LogOutCommand = new DelegateCommand(LogOut);
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate);
            SetThemeModeCommand = new DelegateCommand(themeService.SetThemeMode);
            SetThemeCommand = new DelegateCommand<ThemeItem>(arg => themeService.SetTheme(arg.DisplayName));
        }

        #region 字段/属性

        private bool initialize;

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        public IThemeService themeService { get; set; }
        public IApplicationService appService { get; init; }

        private readonly IAccountService accountService;
        private readonly IRegionManager regionManager;
        public DelegateCommand SetThemeModeCommand { get; }
        public DelegateCommand<ThemeItem> SetThemeCommand { get; }
        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; }
        public DelegateCommand LogOutCommand { get; private set; }

        #endregion

        private async void LogOut()
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
            {
                initialize = false;
                await accountService.LogoutAsync();
            }
        }

        public void Navigate(NavigationItem navigationItem)
        {
            if (navigationItem == null) return;

            Navigate(navigationItem.PageViewName);
        }

        private void Navigate(string pageName)
        {
            var region = regionManager.Regions[AppRegionManager.Main];
            var view = region.Views.FirstOrDefault(q => q.GetType().Name.Equals(pageName));
            if (view == null)
                region.RequestNavigate(pageName, NavigateionCallBack);
            else
            {
                SelectedIndex = region.Views.IndexOf(view);
            }
        }

        public void RemoveView(object view)
        {
            var region = regionManager.Regions[AppRegionManager.Main];
            if (region.Views.Contains(view))
                region.Remove(view);
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