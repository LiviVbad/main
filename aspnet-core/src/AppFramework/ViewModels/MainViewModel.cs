using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Common.Services.Account; 
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        public MainViewModel(
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