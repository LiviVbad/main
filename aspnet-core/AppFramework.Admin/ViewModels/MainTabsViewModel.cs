using AppFramework.Shared;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services;
using AppFramework.Services;
using AppFramework.Services.Notification; 
using Prism.Commands;
using Prism.Regions;
using Syncfusion.UI.Xaml.TreeView;
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
            NavigateCommand = new DelegateCommand<ItemSelectionChangedEventArgs>(Navigate);
            SeeAllNotificationsCommand = new DelegateCommand(() =>
            {
                NotificationPanelIsOpen = false;
                notificationService.SeeAllNotificationsPage();
            });
            SetNotificationAsRead = new DelegateCommand(notificationService.SetNotificationAsRead);
            SetAllNotificationsAsReadCommand = new DelegateCommand(notificationService.SetAllNotificationsAsRead);
            ExecuteUserActionCommand = new DelegateCommand<string>(appService.ExecuteUserAction);
        }

        #region 字段/属性

        public NavigationService NavigationService { get; set; }
        public NotificationService notificationService { get; set; }
        public IApplicationService appService { get; init; }
        public DelegateCommand<ItemSelectionChangedEventArgs> NavigateCommand { get; private set; }
        public DelegateCommand<string> ExecuteUserActionCommand { get; private set; }

        private bool notificationPanelIsOpen;

        public bool NotificationPanelIsOpen
        {
            get { return notificationPanelIsOpen; }
            set
            {
                notificationPanelIsOpen = value;
                if (value)
                {
                    AsyncRunner.Run(notificationService.GetNotifications());
                }
                RaisePropertyChanged();
            }
        }

        public DelegateCommand SeeAllNotificationsCommand { get; private set; }
        public DelegateCommand SettingsCommand { get; private set; }
        public DelegateCommand SetAllNotificationsAsReadCommand { get; private set; }
        public DelegateCommand SetNotificationAsRead { get; private set; }

        #endregion

        public void Navigate(ItemSelectionChangedEventArgs args)
        {
            if (args != null && args.AddedItems != null)
            {
                if (args.AddedItems[0] is NavigationItem item)
                {
                    NavigationService.Navigate(item.PageViewName);
                }
            }
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await appService.GetApplicationInfo();
            NavigationService.Navigate(AppViews.Dashboard);
        }
    }
}