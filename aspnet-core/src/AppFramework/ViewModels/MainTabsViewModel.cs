using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Account;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using Syncfusion.Data.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class MainTabsViewModel : NavigationViewModel
    {
        public MainTabsViewModel(
            IAccountService accountService,
            IRegionManager regionManager,
            IApplicationService appService)
        {
            this.appService = appService;
            this.accountService = accountService;
            this.regionManager = regionManager;

            LogOutCommand = new DelegateCommand(LogOut);
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate);
        }

        #region 字段/属性

        private bool initialize;

        private bool isShowUserPanel;

        public bool IsShowUserPanel
        {
            get { return isShowUserPanel; }
            set { isShowUserPanel = value; RaisePropertyChanged(); }
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        public IApplicationService appService { get; init; }

        private readonly IAccountService accountService;
        private readonly IRegionManager regionManager;
        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; }
        public DelegateCommand LogOutCommand { get; private set; }
         
        #endregion

        private async void LogOut()
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
            {
                initialize = false;
                NavigationRegion.RemoveAll();
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
            var view = NavigationRegion.Views.FirstOrDefault(q => q.GetType().Name.Equals(pageName));
            if (view == null)
                NavigationRegion.RequestNavigate(pageName, NavigateionCallBack);
            else
            {
                SelectedIndex = NavigationRegion.Views.IndexOf(view);
            }
        }

        public void RemoveView(object view)
        {
            if (NavigationRegion.Views.Contains(view))
                NavigationRegion.Remove(view);
        }


        private IRegion NavigationRegion => regionManager.Regions[AppRegionManager.Main];

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