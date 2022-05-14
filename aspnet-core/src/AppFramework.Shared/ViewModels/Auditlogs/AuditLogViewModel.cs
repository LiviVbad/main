using AppFramework.Auditing;
using AppFramework.Auditing.Dto; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Models;

namespace AppFramework.Shared.ViewModels
{
    public class AuditLogViewModel : RegionCurdViewModel<AuditLogListModel>
    {
        private readonly IAuditLogAppService appService;
        public GetAuditLogsInput input;

        public AuditLogViewModel(IAuditLogAppService appService)
        {
            input = new GetAuditLogsInput()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize,
            };
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            CurrentPage = 0;
            input.SkipCount = 0;
            GridModelList.Clear();

            await GetAuditLogAsync();
        }

        public override async void LoadMore()
        {
            if (IsBusy) return;

            if (GridModelList?.Count == TotalCount) return;

            input.SkipCount = AppConsts.DefaultPageSize * ++CurrentPage;

            await GetAuditLogAsync();
        }

        private async Task GetAuditLogAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(() => appService.GetAuditLogs(input),
                           async result =>
                           {
                               foreach (var item in Map<List<AuditLogListModel>>(result.Items))
                                   GridModelList.Add(item);

                               TotalCount = result.TotalCount;

                               await Task.CompletedTask;
                           });
            });
        }
    }
}