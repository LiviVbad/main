using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using System.Threading.Tasks;
using AppFramework.Shared.Models;

namespace AppFramework.Shared.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        private readonly IUserAppService appService;

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

        public UserViewModel(IUserAppService appService)
        {
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService=appService;
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
                await WebRequest.Execute(() => appService.GetUsers(input), dataPager.SetList);
            });
        }

        public override async void Delete(object selectedItem)
        {
            var id = (selectedItem as UserListDto).Id;
            if (!await dialogService.DeleteConfirm()) return;
            await appService.DeleteUser(new EntityDto<long>(id));
            await RefreshAsync();
        }
    }
}