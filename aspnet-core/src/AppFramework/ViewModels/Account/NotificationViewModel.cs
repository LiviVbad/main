using AppFramework.Authorization.Users.Dto;
using AppFramework.Common;
using AppFramework.Notifications;
using AppFramework.Notifications.Dto;
using AppFramework.ViewModels.Shared;
using System;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class NotificationViewModel : NavigationCurdViewModel
    {
        private readonly INotificationAppService appService;
        public GetUserNotificationsInput input;

        public NotificationViewModel(INotificationAppService appService)
        {
            Title = Local.Localize("Notifications");
            this.appService = appService;

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            input = new GetUserNotificationsInput()
            {
                StartDate = new DateTime(year, month, day, 0, 0, 0),
                EndDate = new DateTime(year, month, day, 23, 59, 59),
                MaxResultCount = 10,
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler; ;
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await GetUserNotifications(input);
            });
        }

        private async Task GetUserNotifications(GetUserNotificationsInput filter)
        {
            await WebRequest.Execute(() => appService.GetUserNotifications(filter),
                        async result =>
                        {
                            dataPager.SetList(result);

                            await Task.CompletedTask;
                        });
        }

        public override async Task RefreshAsync()
        {
            dataPager.PageIndex = 0;

            await Task.CompletedTask;
        }
    }
}
