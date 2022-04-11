using Abp.Application.Services.Dto;
using AppFrameworkDemo.Auditing;
using AppFrameworkDemo.Auditing.Dto;
using AppFrameworkDemo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class AuditLogViewModel : NavigationCurdViewModel<AuditLogListModel>
    {
        private readonly IAuditLogAppService appService;
        public GetAuditLogsInput input;

        public AuditLogViewModel(IAuditLogAppService appService)
        {
            input=new GetAuditLogsInput()
            {
                StartDate=DateTime.Now.AddDays(-5),
                EndDate=DateTime.Now
            };
            this.appService=appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(
                       () => appService.GetAuditLogs(input),
                       result => RefreshSuccessed(result));
            }); 
        }

        private async Task RefreshSuccessed(PagedResultDto<AuditLogListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<AuditLogListModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}