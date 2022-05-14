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

        public async void Delete()
        {
            if (await dialog.Question(Local.Localize("RoleDeleteWarningMessage", SelectedItem.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteRole(
                        new EntityDto(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
        }

        public override PermButton[] CreatePermissionButtons()
        {
            return new PermButton[]
            {
                new PermButton(Permkeys.RoleEdit, Local.Localize("Change"),()=>Edit()),
                new PermButton(Permkeys.RoleDelete, Local.Localize("Delete"),()=>Delete())
            };
        }
    }
}