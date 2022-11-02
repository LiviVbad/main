using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using Prism.Navigation;
using System.Threading.Tasks;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Permission;
using AppFramework.Shared.Views;

namespace AppFramework.Shared.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        private readonly IUserAppService appService;
        private readonly IProfileAppService profileService;
         
        public GetUsersInput input { get; set; }

        public UserListModel SelectedItem = null;

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

        public UserViewModel(IUserAppService appService, IProfileAppService profileService)
        {
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService = appService;
            this.profileService = profileService;
        }

        public async void CreateNewUserAsync()
        {
            await GotoUserDetailsAsync(null);
        }

        public async void Delete()
        {
            if (!await dialogService.DeleteConfirm()) return;

            await appService.DeleteUser(new EntityDto<long>()
            {
                Id= SelectedItem.Id
            });
            await RefreshAsync();
        }

        private async Task GotoUserDetailsAsync(UserListModel user)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", user);
            await navigationService.NavigateAsync(AppViews.UserDetails, param);
        }

        private async Task SearchWithDelayAsync(string filterText)
        {
            if (!string.IsNullOrEmpty(filterText))
            {
                await Task.Delay(1000);

                if (filterText != input.Filter)
                    return;
            }
             
            dataPager.SkipCount = 0;

            await RefreshAsync();
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetUsers(input), async result =>
                {
                    dataPager.SetList(result);
                    await Task.CompletedTask;
                });
            });
        }

        protected override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.UserEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.UserDelete, Local.Localize("Delete"),Delete)
            };
        }
    }
}