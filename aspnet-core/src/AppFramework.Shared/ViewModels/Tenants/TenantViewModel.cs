using Abp.Application.Services.Dto;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Core;
using AppFramework.Common.Models;
using AppFramework.Common;

namespace AppFramework.Shared.ViewModels
{
    public class TenantViewModel : RegionCurdViewModel
    {
        private readonly ITenantAppService appService;
        private GetTenantsInput filter;

        public TenantViewModel(ITenantAppService appService, IMessenger messenger)
        {
            filter = new GetTenantsInput()
            {
                EditionIdSpecified = false,
                MaxResultCount = 10,
                SkipCount = 0,
            };
            messenger.Sub(AppMessengerKeys.Tenant, async () => await RefreshAsync());
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(() => appService.GetTenants(filter), RefreshSuccessed);
            });
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is TenantListDto item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.DeleteTenant(new EntityDto()
                {
                    Id = item.Id
                });
                await RefreshAsync();
            }
        }

        private async Task RefreshSuccessed(PagedResultDto<TenantListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in result.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}