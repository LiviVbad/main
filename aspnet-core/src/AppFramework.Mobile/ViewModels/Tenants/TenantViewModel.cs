using Abp.Application.Services.Dto;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto; 
using System.Threading.Tasks;
using AppFramework.Shared.Core;
using AppFramework.Shared.Services.Permission;
using AppFramework.Shared.Models;

namespace AppFramework.Shared.ViewModels
{
    public class TenantViewModel : NavigationCurdViewModel
    {
        private readonly ITenantAppService appService;
        private GetTenantsInput filter;

        public TenantListModel SelectedItem => Map<TenantListModel>(dataPager.SelectedItem);

        public TenantViewModel(ITenantAppService appService)
        {
            filter = new GetTenantsInput()
            {
                EditionIdSpecified = false,
                MaxResultCount = 10,
                SkipCount = 0,
            };
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetTenants(filter), async result =>
                {
                    dataPager.SetList(result);
                    await Task.CompletedTask;
                });
            });
        }

        public async void Delete()
        {
            if (!await dialogService.DeleteConfirm()) return;

            await appService.DeleteTenant(new EntityDto()
            {
                 Id= SelectedItem.Id
            });
            await RefreshAsync();
        }
          
        protected override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            { 
                new PermissionItem(AppPermissions.TenantEdit, Local.Localize("Change"),Edit), 
                new PermissionItem(AppPermissions.TenantDelete, Local.Localize("Delete"),Delete), 
            };
        }
    }
}