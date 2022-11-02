using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using System.Threading.Tasks;
using AppFramework.Shared.Core;
using AppFramework.Shared.Services.Permission;
using AppFramework.Shared.Models;
using ImTools;

namespace AppFramework.Shared.ViewModels
{
    public class RoleViewModel : NavigationCurdViewModel
    {
        private readonly IRoleAppService appService;

        public RoleListModel SelectedItem => Map<RoleListModel>(dataPager.SelectedItem);

        public RoleViewModel(IRoleAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetRoles(new GetRolesInput()), async result =>
                {
                    dataPager.SetList(new PagedResultDto<RoleListDto>
                    {
                        Items = result.Items
                    }); 
                    await Task.CompletedTask;
                });
            });
        }

        public async void Delete()
        {
            if (!await dialogService.DeleteConfirm()) return;

            await appService.DeleteRole(new EntityDto()
            {
                Id= SelectedItem.Id
            });
            await RefreshAsync();
        }

        protected override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.RoleEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.RoleDelete, Local.Localize("Delete"),Delete)
            };
        }
    }
}