using Abp.Notifications;
using AppFramework.Common;
using AppFramework.Notifications;
using AppFramework.Notifications.Dto;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Services.Notification
{
    public class NotificationService : BindableBase
    {
        private readonly INotificationAppService appService;
        private readonly NavigationService navigationService;
        private ObservableCollection<UserNotification> items;

        public ObservableCollection<UserNotification> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        public GetUserNotificationsInput input;

        private bool isunRead;

        public bool IsUnRead
        {
            get { return isunRead; }
            set { isunRead = value; RaisePropertyChanged(); }
        }
         
        public NotificationService(INotificationAppService appService,
            NavigationService navigationService)
        {
            this.appService = appService;
            this.navigationService = navigationService;
            items = new ObservableCollection<UserNotification>();
            input = new GetUserNotificationsInput()
            {
                MaxResultCount = 3,
            };
        }

        public async Task GetNotifications()
        {
            await WebRequest.Execute(async () => await appService.GetUserNotifications(input),
                async output =>
                {
                    Items.Clear();

                    IsUnRead = output.UnreadCount > 0 ? true : false;

                    foreach (var item in output.Items)
                        Items.Add(item);

                    await Task.CompletedTask;
                });
        }

        public void Settings()
        { }

        public void SeeAllNotifications()
        {
            navigationService.Navigate(AppViewManager.Notification);
        }

        public async void SetAllNotificationsAsRead()
        {
            await WebRequest.Execute(async () => await appService.SetAllNotificationsAsRead(), GetNotifications);
        }

        public void SetNotificationAsRead()
        { }
    }
}
