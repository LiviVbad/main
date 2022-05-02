using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
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
                      result => RefreshSuccessed(result));
            });
        }

        private async Task RefreshSuccessed(PagedResultDto<TenantListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<TenantListModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}
