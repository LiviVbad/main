using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto; 
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class TenantViewModel : NavigationCurdViewModel<TenantListModel>
    {
        private readonly ITenantAppService appService;
        private GetTenantsInput filter;

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
                await WebRequest.Execute(
                      () => appService.GetTenants(filter),
                      async result =>
                      {
                          GridModelList.Clear();

                          foreach (var item in Map<List<TenantListModel>>(result.Items))
                              GridModelList.Add(item);

                          await Task.CompletedTask;
                      });
            });
        }

        private async void TenantChangeFeatures(int Id)
        {
            await dialog.ShowDialogAsync(AppViewManager.TenantChangeFeatures);
        }

        private async void Delete()
        {
            var result = await dialog.Question(Local.Localize("TenantDeleteWarningMessage", SelectedItem.TenancyName));
            if (result)
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteTenant(
                        new EntityDto(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
        }

        private void TenantImpersonation() { }

        public override PermButton[] CreatePermissionButtons()
        {
            return new PermButton[]
             {
                new PermButton(Permkeys.TenantImpersonation, Local.Localize("LoginAsThisTenant"),()=>TenantImpersonation()),
                new PermButton(Permkeys.TenantEdit, Local.Localize("Change"),()=>Edit()),
                new PermButton(Permkeys.TenantChangeFeatures, Local.Localize("Features"),()=>TenantChangeFeatures(SelectedItem.Id)),
                new PermButton(Permkeys.TenantDelete, Local.Localize("Delete"),()=>Delete()),
                new PermButton(Permkeys.TenantEdit, Local.Localize("Unlock"),null)
             };
        }
    }
}
