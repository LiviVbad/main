using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class AuditLogsViewModel : NavigationCurdViewModel<AuditLogListModel>
    {
        public GetAuditLogsInput input;
        private readonly IAuditLogAppService appService;

        public DelegateCommand ViewCommand { get; private set; }

        public AuditLogsViewModel(IAuditLogAppService appService)
        {
            input = new GetAuditLogsInput()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize,
            };
            this.appService = appService;

            ViewCommand = new DelegateCommand(ViewLogs);
        }

        private void ViewLogs()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", SelectedItem);
            dialog.ShowDialogAsync(AppViewManager.AuditLogDetails, param);
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