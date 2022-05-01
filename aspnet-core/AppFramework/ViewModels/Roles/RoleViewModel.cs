using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Localization;
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
                        result => RefreshSuccessed(result));
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

        private async Task RefreshSuccessed(ListResultDto<RoleListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<RoleListModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}