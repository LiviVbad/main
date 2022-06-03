using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using Prism.Navigation;
using System.Threading.Tasks;
using AppFramework.Common.Models;
using AppFramework.Common.Core;
using AppFramework.Common;

namespace AppFramework.Shared.ViewModels
{
    public class UserViewModel : RegionCurdViewModel
    {
        #region 字段/属性

        private readonly IUserAppService appService;
        private readonly IProfileAppService profileService;

        private int currentPage;
        private int totalUsersCount;
        public GetUsersInput input { get; set; }

        public string FilterText
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                RaisePropertyChanged();
                AsyncRunner.Run(SearchWithDelayAsync(value));
            }
        }

        #endregion 字段/属性

        public UserViewModel(
            IUserAppService appService,
            IProfileAppService profileService,
            IMessenger messenger)
        {
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            messenger.Sub(AppMessengerKeys.User, async () => await RefreshAsync());
            this.appService = appService;
            this.profileService = profileService;
        }

        public async void CreateNewUserAsync()
        {
            await GotoUserDetailsAsync(null);
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is UserListDto item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.DeleteUser(new EntityDto<long>()
                {
                    Id = item.Id
                });
                await RefreshAsync();
            }
        }

        private async Task GotoUserDetailsAsync(UserListModel user)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", user);
            await navigationService.NavigateAsync(AppViewManager.UserDetails, param);
        }

        private async Task SearchWithDelayAsync(string filterText)
        {
            if (!string.IsNullOrEmpty(filterText))
            {
                await Task.Delay(1000);

                if (filterText != input.Filter)
                    return;
            }

            currentPage = 0;
            input.SkipCount = 0;

            await RefreshAsync();
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(() => appService.GetUsers(input), RefreshSuccessed);
            });
        }

        private async Task RefreshSuccessed(PagedResultDto<UserListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in result.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}