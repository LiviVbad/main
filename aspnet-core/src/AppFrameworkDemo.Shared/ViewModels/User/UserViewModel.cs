using Abp.Application.Services.Dto;
using AppFrameworkDemo.Authorization.Users;
using AppFrameworkDemo.Authorization.Users.Dto;
using AppFrameworkDemo.Authorization.Users.Profile;
using AppFrameworkDemo.Shared.Models;
using Prism.Navigation; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel<UserListModel>
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

        public override async void Delete(UserListModel selectedItem)
        {
            if (selectedItem == null) return;

            if (!await dialogService.DeleteConfirm()) return;

            await appService.DeleteUser(new EntityDto<long>()
            {
                Id = selectedItem.Id
            });
            await RefreshAsync();
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
                await WebRequestRuner.Execute(
                       () => appService.GetUsers(input),
                       result => RefreshSuccessed(result));
            });
        }

        private async Task RefreshSuccessed(PagedResultDto<UserListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<UserListModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}