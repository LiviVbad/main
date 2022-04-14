using Abp.Application.Services.Dto;
using AppFrameworkDemo.Authorization.Roles;
using AppFrameworkDemo.Authorization.Roles.Dto;
using AppFramework.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class RoleViewModel : NavigationCurdViewModel<RoleListModel>
    {
        private readonly IRoleAppService appService;

        public RoleViewModel(IRoleAppService appService,IMessenger messenger)
        {
            messenger.Sub(AppMessengerKeys.Role, async () => await RefreshAsync());
            this.appService=appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(
                        () => appService.GetRoles(new GetRolesInput()),
                        result => RefreshSuccessed(result));
            }); 
        }

        public override async void Delete(RoleListModel selectedItem)
        {
            if (selectedItem==null) return;

            if (!await dialogService.DeleteConfirm()) return;

            await appService.DeleteRole(new EntityDto()
            {
                Id=selectedItem.Id
            });
            await RefreshAsync();
        }

        private async Task RefreshSuccessed(ListResultDto<RoleListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<RoleListModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}