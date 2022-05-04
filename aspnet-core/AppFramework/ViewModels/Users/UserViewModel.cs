using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Services.Permission;

namespace AppFramework.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel<UserListModel>
    {
        private readonly IUserAppService appService;
        public readonly IProfileAppService profileAppService;

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

        public UserViewModel(
            IUserAppService appService,
            IProfileAppService profileAppService)
        {
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService = appService;
            this.profileAppService = profileAppService;
        }

        private async Task SearchWithDelayAsync(string filterText)
        {
            if (!string.IsNullOrEmpty(filterText))
            {
                await Task.Delay(1000);

                if (filterText != input.Filter)
                    return;
            }

            input.SkipCount = 0;

            await RefreshAsync();
        }

        public override void CreateDefaultButtons()
        {
            PermissionButtons.Add(new PermissionButton("", Local.Localize("LoginAsThisUser")));
            PermissionButtons.Add(new PermissionButton("", Local.Localize("Change")));
            PermissionButtons.Add(new PermissionButton("", Local.Localize("Permissions")));
            PermissionButtons.Add(new PermissionButton("", Local.Localize("Unlock")));
            PermissionButtons.Add(new PermissionButton("", Local.Localize("Delete")));
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
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