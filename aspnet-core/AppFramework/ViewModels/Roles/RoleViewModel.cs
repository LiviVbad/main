using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class RoleViewModel : NavigationCurdViewModel<RoleListModel>
    {
        private readonly IRoleAppService appService;

        public RoleViewModel(IRoleAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                        () => appService.GetRoles(new GetRolesInput()),
                        async result =>
                        {
                            GridModelList.Clear();

                            foreach (var item in Map<List<RoleListModel>>(result.Items))
                                GridModelList.Add(item);

                            await Task.CompletedTask;
                        });
            });
        }

        public override async void Delete(RoleListModel selectedItem)
        {
            if (selectedItem == null) return;

            if (!await dialog.Question(Local.Localize(""))) return;

            await appService.DeleteRole(new EntityDto()
            {
                Id = selectedItem.Id
            });
            await RefreshAsync();
        }

        public override PermissionButton[] CreatePermissionButtons()
        {
            return new PermissionButton[]
            {
                new PermissionButton(PermissionKey.RoleEdit, Local.Localize("Change")),
                new PermissionButton(PermissionKey.RoleDelete, Local.Localize("Delete"))
            };
        }
    }
}