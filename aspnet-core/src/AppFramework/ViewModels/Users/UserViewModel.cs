using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Services.Permission;
using Abp.Application.Services.Dto;
using Prism.Services.Dialogs;
using AppFramework.Common.Services.Account;

namespace AppFramework.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel<UserListModel>
    {
        public UserViewModel(IUserAppService appService,
            IAccountService accountService,
            IProfileAppService profileAppService)
        {
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService = appService;
            this.accountService = accountService;
            this.profileAppService = profileAppService;
        }

        private readonly IUserAppService appService;
        private readonly IAccountService accountService;
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
         
        #region 修改权限/解锁/使用当前账户登录

        private async void UserChangePermission(int Id)
        {
            GetUserPermissionsForEditOutput output = null;
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                appService.GetUserPermissionsForEdit(new EntityDto<long>(Id)),
                async result =>
                {
                    output = result;
                    await Task.CompletedTask;
                });
            });

            if (output != null)
            {
                DialogParameters param = new DialogParameters();
                param.Add("Id", Id);
                param.Add("Value", output);
                await dialog.ShowDialogAsync(AppViewManager.UserChangePermission, param);
            }
        }

        private async void UsersUnlock(int Id)
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(() => appService.UnlockUser(
                     new EntityDto<long>(Id)));
             });
        }

        private async void LoginAsThisUser()
        {
            await accountService.LoginCurrentUserAsync(SelectedItem);
        }

        public async void Delete()
        { 
            if (await dialog.Question(Local.Localize("UserDeleteWarningMessage", SelectedItem.UserName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteUser(
                        new EntityDto<long>(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
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

        public override PermButton[] CreatePermissionButtons()
        {
            return new PermButton[]
            {
                new PermButton(Permkeys.Users, Local.Localize("LoginAsThisUser"),()=>LoginAsThisUser()),
                new PermButton(Permkeys.UserEdit, Local.Localize("Change"),()=>Edit()),
                new PermButton(Permkeys.UserChangePermission, Local.Localize("Permissions"),()=>UserChangePermission(SelectedItem.Id)),
                new PermButton(Permkeys.UsersUnlock, Local.Localize("Unlock"),()=>UsersUnlock(SelectedItem.Id)),
                new PermButton(Permkeys.UserDelete, Local.Localize("Delete"),()=>Delete())
            };
        }
    }
}