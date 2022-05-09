using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Services.Permission;
using Abp.Application.Services.Dto;

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

        public override void Execute(string obj)
        {
            switch (obj)
            {
                case PermissionKey.Users: LoginAsThisUser(); break;
                case PermissionKey.UserEdit: Edit(); break;
                case PermissionKey.UserChangePermission: UserChangePermission(); break;
                case PermissionKey.UsersUnlock: UsersUnlock(); break;
                case PermissionKey.UserDelete: Delete(); break;
            }
        }

        #region 修改权限/解锁/使用当前账户登录

        private void UserChangePermission() { }

        private async void UsersUnlock()
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(() => appService.UnlockUser(
                     new EntityDto<long>(SelectedItem.Id)));
             });
        }

        private void LoginAsThisUser() { }

        public override async void Delete()
        {
            var dialogResult = await dialog.Question(Local.Localize("UserDeleteWarningMessage", SelectedItem.UserName));

            if (dialogResult)
            { }
        }

        #endregion

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

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                       () => appService.GetUsers(input),
                       async result =>
                       {
                           GridModelList.Clear();

                           foreach (var item in Map<List<UserListModel>>(result.Items))
                               GridModelList.Add(item);

                           await Task.CompletedTask;
                       });
            });
        }

        public override PermissionButton[] CreatePermissionButtons()
        {
            return new PermissionButton[]
            {
                new PermissionButton(PermissionKey.Users, Local.Localize("LoginAsThisUser")),
                new PermissionButton(PermissionKey.UserEdit, Local.Localize("Change")),
                new PermissionButton(PermissionKey.UserChangePermission, Local.Localize("Permissions")),
                new PermissionButton(PermissionKey.UsersUnlock, Local.Localize("Unlock")),
                new PermissionButton(PermissionKey.UserDelete, Local.Localize("Delete"))
            };
        }
    }
}