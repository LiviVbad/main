using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override PermissionButton[] CreatePermissionButtons()
        {
            return new PermissionButton[]
             {
                new PermissionButton(PermissionKey.TenantImpersonation, Local.Localize("LoginAsThisTenant")),
                new PermissionButton(PermissionKey.TenantEdit, Local.Localize("Change")),
                new PermissionButton(PermissionKey.TenantChangeFeatures, Local.Localize("Features")),
                new PermissionButton(PermissionKey.TenantDelete, Local.Localize("Delete")),
                new PermissionButton(PermissionKey.TenantEdit, Local.Localize("Unlock"))
             };
        }
    }
}
