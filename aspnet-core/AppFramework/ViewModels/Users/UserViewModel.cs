using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Services.Permission;
using Prism.Commands;

namespace AppFramework.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel<UserListModel>
    {
        private readonly IUserAppService appService;
        public readonly IProfileAppService profileAppService;
        public DelegateCommand<string> ExecuteCommand { get; private set; }

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

            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case PermissionKey.Users: break;
                case PermissionKey.UserEdit: break;
                case PermissionKey.UserChangePermission: break;
                case PermissionKey.UsersUnlock: break;
                case PermissionKey.UserDelete: break;
            }
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