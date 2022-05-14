using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class AuditLogsViewModel : NavigationCurdViewModel<AuditLogListModel>
    {
        public GetAuditLogsInput input;
        private readonly IAuditLogAppService appService;

        public AuditLogsViewModel(IAuditLogAppService appService)
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
            input.SkipCount = 0;
            GridModelList.Clear();

            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAuditLogs(input),
                           async result =>
                           {
                               foreach (var item in Map<List<AuditLogListModel>>(result.Items))
                                   GridModelList.Add(item);

                               await Task.CompletedTask;
                           });
            });
        }
    }
}