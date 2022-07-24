using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services;
using AppFramework.Services;
using AppFramework.Services.Notification;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class MainTabsViewModel : NavigationViewModel
    {
        public MainTabsViewModel(
            NotificationService notificationService,
            NavigationService navigationService,
            IApplicationService appService)
        {
            this.notificationService = notificationService;
            NavigationService = navigationService;
            this.appService = appService;

            SettingsCommand = new DelegateCommand(notificationService.Settings);
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate);
            SeeAllNotificationsCommand = new DelegateCommand(notificationService.SeeAllNotifications);
            SetNotificationAsRead = new DelegateCommand(notificationService.SetNotificationAsRead);
            SetAllNotificationsAsReadCommand = new DelegateCommand(notificationService.SetAllNotificationsAsRead);
            ExecuteUserActionCommand = new DelegateCommand<string>(appService.ExecuteUserAction);
        }

        #region 字段/属性

        public NavigationService NavigationService { get; set; }
        public NotificationService notificationService { get; set; }
        public IApplicationService appService { get; init; }
        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; }
        public DelegateCommand<string> ExecuteUserActionCommand { get; private set; }

        public DelegateCommand SeeAllNotificationsCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }
        public DelegateCommand SetAllNotificationsAsReadCommand { get; private set; }
        public DelegateCommand SetNotificationAsRead { get; private set; }

        #endregion

        public void Navigate(NavigationItem navigationItem)
        {
            NavigationService.Navigate(navigationItem.PageViewName);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await appService.GetApplicationInfo();
            NavigationService.Navigate(AppViewManager.Dashboard);
            await notificationService.GetNotifications();
        }
    }
}