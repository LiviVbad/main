using Abp.Notifications;
using AppFramework.Common;
using AppFramework.Notifications;
using AppFramework.Notifications.Dto;
using Prism.Mvvm; 
using System.Collections.ObjectModel; 
using System.Threading.Tasks;

namespace AppFramework.Services.Notification
{
    public class NotificationService : BindableBase
    {
        private readonly INotificationAppService appService;

        private ObservableCollection<UserNotification> items;

        public ObservableCollection<UserNotification> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        public GetUserNotificationsInput input;

        public NotificationService(INotificationAppService appService)
        {
            this.appService = appService;
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

                    foreach (var item in output.Items)
                        Items.Add(item);

                    await Task.CompletedTask;
                });
        }

        public void Settings()
        {

        }

        public void GetAllNotifications()
        {

        }

        public void SetAllNotificationsAsRead()
        {
        }

        public void SetNotificationAsRead()
        {

        }
    }
}
