using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Models;
using AppFramework.Common.Core;
using AppFramework.Common;

namespace AppFramework.Shared.ViewModels
{
    public class RoleViewModel : RegionCurdViewModel
    {
        private readonly IRoleAppService appService;

        public RoleViewModel(IRoleAppService appService, IMessenger messenger)
        {
            messenger.Sub(AppMessengerKeys.Role, async () => await RefreshAsync());
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetRoles(new GetRolesInput()), RefreshSuccessed);
            });
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is RoleListDto item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.DeleteRole(new EntityDto()
                {
                    Id = item.Id
                });
                await RefreshAsync();
            }
        }

        private async Task RefreshSuccessed(ListResultDto<RoleListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in result.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}