using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Services;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class MainTabsViewModel : NavigationViewModel
    {
        public MainTabsViewModel(
            NavigationService navigationService,
            IApplicationService appService)
        {
            NavigationService = navigationService;
            this.appService = appService;

            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate);
            ExecuteUserActionCommand = new DelegateCommand<string>(appService.ExecuteUserAction);
        }

        #region 字段/属性
          
        public NavigationService NavigationService { get; set; }
        public IApplicationService appService { get; init; }
        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; }
        public DelegateCommand<string> ExecuteUserActionCommand { get; private set; }

        #endregion

        public void Navigate(NavigationItem navigationItem)
        {
            NavigationService.Navigate(navigationItem.PageViewName);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await appService.GetApplicationInfo();
            NavigationService.Navigate(AppViewManager.Dashboard);
        }
    }
}